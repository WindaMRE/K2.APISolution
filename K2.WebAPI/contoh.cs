using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace K2.WebAPI
{
    public class contoh
    {
        public async Task<(bool Authenticated, string Message)> AuthenticateAsync(string username, string password, bool useCert = false)
        {
            int failedCount = 0;
            try
            {

                int maxTryLogin = Helpers.Settings.AppSettingValue("AppSettings", "MaxTryUntilLock", 3);

                var u = repository.GetQueryable<AspNetUsers>().Include(i => i.AspNetUsersRoles).Where(x => x.UserName == username).OrderBy(x => x.Id);

                // return null if user not found
                if (!u.Any())
                {
                    var msgFailed = await repository.GetMsgFromDBAsync("MWOSSTD047E");

                    //return (false, "Invalid username or password");
                    return (false, msgFailed);
                }
                else
                {
                    var u2 = u.Where(x => x.PasswordHash == password).OrderBy(x => x.Id);
                    if (u2.Any())
                    {
                        if (u.FirstOrDefault().LockoutEnabled)
                        {
                            var msgDisabled = await repository.GetMsgFromDBAsync("MWOSSTD050I");
                            //return (false, "Username disabled");
                            return (false, msgDisabled);
                        }
                    }
                    else
                    {

                        failedCount = u.FirstOrDefault().AccessFailedCount + 1;
                        var userF = u.FirstOrDefault();

                        userF.AccessFailedCount = failedCount;
                        if (failedCount >= maxTryLogin)
                        {
                            userF.LockoutEnabled = true;
                        }
                        var (UpdatedF, MessageF) = await repository.UpdateAsyncForUser(userF);

                        var msgInvalid = await repository.GetMsgFromDBAsync("MWOSA06014E", userF.UserName, userF.LastLoginDate);

                        //return (false, "Invalid username or password");
                        return (false, msgInvalid);
                    }
                }

                var user = u.FirstOrDefault();


                var users = repository.GetQueryable<AspNetUsersRoles>(x => x.Role, x => x.User, x => x.User.UsersDetails).Where(x => x.UserId == user.Id).OrderBy(x => x.Id);

                var aspuserolename = users.Select(s => s.Role.Name).ToList();
                var aspuserole = users.Select(s => s.Role).ToList();

                var userdetailTemp = users.Select(s => s.User.UsersDetails).FirstOrDefault();

                var userdetail = new UsersDetails();
                if (userdetailTemp != null)
                {
                    userdetail = userdetailTemp;
                }

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Helpers.Settings.AppSettingValue("AppSettings", "Secret", "7E28DE07-E644-4D2A-A8A2-D00F9A0EF31E"));
                string serverPath = Helpers.Settings.AppSettingValue("Server", "APIUrl", "http://localhost:5000");
                string directoryPath = Helpers.Settings.AppSettingValue("Upload", "UserProfile", "wwwroot\\Uploads\\Img\\UserPic\\").Replace("\\", "/").Replace("wwwroot", ""); ;
                int TokenExpired = Helpers.Settings.AppSettingValue("AppSettings", "TokenExpired", 180);
                int RefreshTokenExpired = Helpers.Settings.AppSettingValue("AppSettings", "RefreshTokenExpired", 10080);
                string Issuer = Helpers.Settings.AppSettingValue("AppSettings", "Issuer", "My Issuer");
                string Audience = Helpers.Settings.AppSettingValue("AppSettings", "Audience", "apps-api");
                string hash = Helpers.Settings.AppSettingValue("AppSettings", "AES-256-CBC", "GZxBUTzzin3GYC4rlQ6dxVFA3TxxJfYv");

                string givenName = "N/A";
                string family_name = "";
                string initialName = "";
                string avatar = "";
                string fullName = "N/A";
                string refreshToken = "";
                string emailConfirmed = user.EmailConfirmed.ToString();

                if (userdetail != null)
                {
                    givenName = userdetail.FirstName;
                    family_name = userdetail.LastName;
                    fullName = givenName + family_name;
                    emailConfirmed = user.EmailConfirmed.ToString();
                    if (!string.IsNullOrEmpty(user.SecurityStamp))
                    {
                        refreshToken = user.SecurityStamp.ToString();
                    }

                    if (!string.IsNullOrEmpty(userdetail.InitialName))
                    {
                        initialName = userdetail.InitialName.ToString();
                    }

                    if (!string.IsNullOrEmpty(userdetail.Avatar))
                    {
                        avatar = userdetail.Avatar.ToString();
                    }
                    else
                    {
                        avatar = "avatar.png";
                    }
                }

                if (string.IsNullOrEmpty(givenName))
                {
                    givenName = "";
                }

                if (string.IsNullOrEmpty(family_name))
                {
                    family_name = "";
                }

                if (string.IsNullOrEmpty(fullName))
                {
                    fullName = "";
                }

                if (string.IsNullOrEmpty(initialName))
                {
                    initialName = "";
                }

                if (string.IsNullOrEmpty(avatar))
                {
                    avatar = "";
                }

                //generate refresh token and base64 them
                Guid refToken = Guid.NewGuid();
                byte[] encodedBytes = System.Text.Encoding.Unicode.GetBytes(refToken.ToString());
                string base64refToken = Convert.ToBase64String(encodedBytes);
                string fullname = givenName.ToString();

                if (family_name.ToString() != "")
                {
                    fullname = givenName.ToString() + " " + family_name.ToString();
                }

                var claims = new List<Claim> {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                            new Claim(JwtRegisteredClaimNames.UniqueName, fullName),
                            new Claim("initialName", fullname),
                            new Claim("avatar", serverPath.ToString() + directoryPath.ToString() + avatar.ToString()),
                            new Claim("userId", fullname + "|" + user.Id),
                            new Claim(JwtRegisteredClaimNames.GivenName, givenName.ToString()),
                            new Claim(JwtRegisteredClaimNames.FamilyName , family_name.ToString()),
                            new Claim("email_confirmed" , emailConfirmed),
                            new Claim("refreshToken" , base64refToken),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email)
                            };

                foreach (var ar in aspuserolename)
                {
                    claims.Add(new Claim(ClaimTypes.Role, ar));
                }

                foreach (var ar in aspuserole)
                {
                    var _asppermissionrole = await repository.ListAsync<AspNetPermissionsRoles>();
                    var asppermissionrole = _asppermissionrole.Where(x => x.RoleId == ar.Id).Select(s => s.Name);

                    foreach (var apr in asppermissionrole)
                    {
                        if (apr != null)
                        {
                            claims.Add(new Claim(CustomClaimTypes.Permission, apr));
                        }
                    }

                    //claims.Add(new Claim(CustomClaimTypes.Permission, ar.Name));
                }

                TokenExpired = TokenExpired <= 0 ? TokenExpired = 60 : TokenExpired;

                DateTime dtNow = Helpers.Others.DateTimeConvertToZone(DateTime.Now);

                user.SecurityStamp = base64refToken;

                RefreshTokenExpired = RefreshTokenExpired <= 0 ? RefreshTokenExpired = 30 : RefreshTokenExpired;

                user.LockoutEndDateUtc = dtNow.AddMinutes(RefreshTokenExpired);

                user.LastLoginDate = DateTime.Now;
                user.AccessFailedCount = 0;
                var (Updated, Message) = await repository.UpdateAsyncForUser(user);

                if (!Updated && Message != null)
                {
                    return (false, Message);
                }

                //prevent save di DB for token
                var token = new JwtSecurityToken(issuer: Issuer,
                                audience: Audience,
                                claims: claims,
                                expires: DateTime.Now.AddMinutes(TokenExpired),
                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                            );

                if (!useCert)
                {
                    user.Token = tokenHandler.WriteToken(token);
                }
                else
                {
                    //user.Token = Helpers.Others.EncryptString(tokenHandler.WriteToken(token), hash, Helpers.Others.RandomString(16));
                    user.Token = Helpers.Others.CompressString(tokenHandler.WriteToken(token));
                }

                //logger.LogInformation(user.Token);
                return (true, user.Token);
            }
            catch (Exception ex)
            {
                logger.LogError("Data failed to auth with error : " + ex.Message, ex.InnerException);
                return (false, "Trouble happened! \n" + ex.Message);
            }
        }


    }
}