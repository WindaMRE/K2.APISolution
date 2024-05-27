using K2.WebAPI.Service;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using static K2.WebAPI.Models.ProductModel;
using static K2.WebAPI.Service.Helper;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using NLog;
using System.Xml.Linq;
using System.Drawing;
using iTextSharp.text.pdf;
using Dapper;
using System.Data;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Drawing.Imaging;

namespace K2.WebAPI.Controllers
{
    [System.Web.Http.RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private readonly Logger logger = LogManager.GetLogger(string.Empty);
        private readonly ProductService _prodserv;
        public ProductsController() => _prodserv = new ProductService();

        //protected IHttpActionResult GetSoapFrom(string ChannelName, string ChannelToken, string RequestorID, int WFID, string Profilename, int RecId, int verId, string Activity, string Signature)
        //{
        //    try
        //    {
        //        RestClient client = new(new CUtility().DocuFloDownloadDoc);
        //        RestRequest request = new()
        //        {
        //            Method = Method.POST
        //        };
        //        request.AddHeader("Content-Type", "text/xml");
        //        var body = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
        //                       <soap:Header>
        //                        <Authentication xmlns=""http://tempuri.org/"">
        //                          <ChannelName>" + ChannelName + @"</ChannelName>
        //                          <ChannelToken>" + ChannelToken + @"</ChannelToken>
        //                        </Authentication>
        //                      </soap:Header>
        //                      <soap:Body>
        //                        <DownloadDoc xmlns=""http://tempuri.org/"">
        //                            <reqData>
        //                                <RequestorID>" + RequestorID + @"</RequestorID>
        //                                <ProfileName>" + Profilename + @"</ProfileName>
        //                                <VerID>" + verId + @"</VerID>
        //                            </reqData>
        //                        </DownloadDoc>
        //                      </soap:Body>
        //                     </soap:Envelope>";
        //        request.AddParameter("text/xml", body, ParameterType.RequestBody);
        //        var response = client.ExecuteAsPost(request, request.Method.ToString());
        //        if (response.Content.Contains("html"))
        //        {
        //            var ResultsHTML = new
        //            {
        //                ProcessSuccess = false,
        //                InfoMessage = response.StatusCode.ToString()
        //            };

        //            return Json(ResultsHTML);
        //        }

        //        if (response.ResponseStatus != ResponseStatus.Completed)
        //        {
        //            var ResultsError = new
        //            {
        //                ProcessSuccess = false,
        //                InfoMessage = response.StatusCode.ToString()
        //            };


        //            return Json(ResultsError);
        //            logger.Error(response.ResponseStatus.ToString() + " Line 161");
        //        }

        //        var signatures = Signatures(ChannelName, ChannelToken, RequestorID, WFID, Profilename, RecId, verId, response.Content, Activity, Signature);

        //        if (signatures.ToString() == "Faulted")
        //        {
        //            var ResultsError = new
        //            {
        //                ProcessSuccess = false,
        //                InfoMessage = "Error"
        //            };
        //            logger.Error(signatures.ToString());
        //            return Json(ResultsError);
        //        }

        //        var Results = new
        //        {
        //            ProcessSuccess = true,
        //            InfoMessage = "Sukses"
        //        };
        //        return Json(Results);
        //    }
        //    catch (Exception ex)
        //    {
        //        var Results = new
        //        {
        //            ProcessSuccess = false,
        //            InfoMessage = ex.Message
        //        };
        //        return Json(Results);
        //    }
        //}

        //protected IHttpActionResult Signatures(string ChannelName, string ChannelToken, string RequestorID, int WFID, string Profilename, int RecId, int verId, string SoapResult, string Activity, string Signature)
        //{
        //    var Outfolder = System.AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", "/") + "Output";
        //    if (!Directory.Exists(Outfolder))
        //    {
        //        Directory.CreateDirectory(Outfolder);
        //    }

        //    try
        //    {
        //        var input = XDocument.Parse(SoapResult);
        //        if (SoapResult.Contains("html"))
        //        {
        //            throw new Exception("Docuflo " + ResponseStatus.Error.ToString());
        //        }

        //        var uri = input.Descendants().ToList().Where(a => a.Name.LocalName == "FileURL").FirstOrDefault()?.Value;
        //        var Fname = input.Descendants().ToList().Where(a => a.Name.LocalName == "FileName").FirstOrDefault()?.Value;
        //        string[] FileNameCurrent = input.Descendants().ToList().Where(a => a.Name.LocalName == "FileName").FirstOrDefault()?.Value.Split(new Char[] { '.' });
        //        var FNameNew = FileNameCurrent.FirstOrDefault();

        //        var FullFname = Outfolder + "/" + Fname;
        //        if (uri != null)
        //        {
        //            WebClient myWebClient = new WebClient();
        //            myWebClient.DownloadFile(uri, FullFname);
        //            myWebClient.Dispose();

        //            var ResultSignature = MergeSignaturePDFAsync(FullFname, Signature, Outfolder.ToString(), Fname, Activity);

        //            if (ResultSignature == "" || ResultSignature == "Error")
        //            {
        //                logger.Error("ProsesCheckout line 197");
        //            }
        //            else
        //            {
        //                logger.Error("Sukses Line 204");
        //            }

        //            var ResultCheckout = ProcessCheckOut(ChannelName, ChannelToken, RequestorID, Profilename, RecId, verId);

        //            //ResultCheckout = ResultCheckout.Replace("\r\n", string.Empty);

        //            if (ResultCheckout.Result.ToString() != "Completed")
        //            {
        //                throw new Exception(ResultCheckout.ToString());
        //                logger.Error(ResultCheckout.ToString() + " Error ProsesCheckout line 265");
        //            }
        //            else
        //            {
        //                logger.Error(ResultCheckout + " Line 269");
        //            }

        //            var ResultCheckin = ProcessCheckin(ChannelName, ChannelToken, RequestorID, WFID, Profilename, Fname, ResultSignature, RecId, verId);

        //            if (ResultCheckin.Result == "Error")
        //            {
        //                throw new Exception(ResultCheckin.Result.ToString());
        //            }

        //            var ResultVersioning = GetVersioning(ChannelName, ChannelToken, WFID, RequestorID, Profilename, RecId);

        //            var test = JsonConvert.DeserializeObject<ResultGen>(ResultVersioning.ToString());

        //            return ResultVersioning;
        //        }
        //        else
        //        {
        //            var Message = input.Descendants().ToList().Where(a => a.Name.LocalName == "ErrMsg").FirstOrDefault()?.Value;
        //            var ResultsErrMsg = new
        //            {
        //                ProcessSuccess = false,
        //                InfoMessage = Message
        //            };
        //            return Json(ResultsErrMsg);
                    
        //        }

        //        var Results = new
        //        {
        //            ProcessSuccess = true,
        //            InfoMessage = "Sukses"
        //        };
        //        return Json(Results);
        //    }
        //    catch (Exception ex)
        //    {
        //        var Results = new
        //        {
        //            ProcessSuccess = false,
        //            InfoMessage = ex.Message
        //        };
        //        return Json(Results);
        //    }
        //}

        protected async Task<HttpResponseMessage> GetSoapFrom(string ChannelName, string ChannelToken, string RequestorID, int WFID, string Profilename, int RecId, int verId, string Activity, string Signature)
        {

            ResultGen result = new ResultGen();

            try
            {

                //RestClient client = new("https://ecm-uat-dms.eigerindo.co.id/Docuflo7_WebApi/Docuflo7_WebApi.asmx?op=DownloadDoc");
                RestClient client = new(new CUtility().DocuFloDownloadDoc);

                RestRequest request = new()
                {
                    Method = Method.POST
                };
                request.AddHeader("Content-Type", "text/xml");
                var body = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                           <soap:Header>
                            <Authentication xmlns=""http://tempuri.org/"">
                              <ChannelName>" + ChannelName + @"</ChannelName>
                              <ChannelToken>" + ChannelToken + @"</ChannelToken>
                            </Authentication>
                          </soap:Header>
                          <soap:Body>
                            <DownloadDoc xmlns=""http://tempuri.org/"">
                                <reqData>
                                    <RequestorID>" + RequestorID + @"</RequestorID>
                                    <ProfileName>" + Profilename + @"</ProfileName>
                                    <VerID>" + verId + @"</VerID>
                                </reqData>
                            </DownloadDoc>
                          </soap:Body>
                         </soap:Envelope>";
                request.AddParameter("text/xml", body, ParameterType.RequestBody);
                var response = client.ExecuteAsPost(request, request.Method.ToString());


                if (response.Content.Contains("html"))
                {
                    result.Status = response.StatusCode.ToString();
                    result.Message = response.Content;

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result.Status, result.Message });
                }

                if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    result.Status = HttpStatusCode.InternalServerError.ToString();//response.ResponseStatus.ToString();
                    result.Message = "Docuflo is Error";

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result.Status, result.Message });
                    logger.Error(response.ResponseStatus.ToString() + " Line 161");
                }

                var signatures = Signatures(ChannelName, ChannelToken, RequestorID, WFID, Profilename, RecId, verId, response.Content, Activity, Signature);

                if (signatures.Status.ToString() == "Faulted")
                {
                    logger.Error(signatures.Exception.Message);
                    result.Status = HttpStatusCode.InternalServerError.ToString();//signatures.Status.ToString();
                    result.Message = signatures.Result.ToString();

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result.Status, result.Message });
                }

                ProductService ps = new ProductService();

                result.Status = signatures.Status.ToString();
                //var Results = ps.ServicePOST<GenerateSignatures>(signatures);
                result.Message = "Signature Generated!";


                return Request.CreateResponse(HttpStatusCode.OK, new { result.Status, result.Message });
                logger.Error("Line 166");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result.Status = ResponseStatus.Error.ToString();
                result.Message = ex.Message.ToString();

                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result.Status, result.Message });
            }
        }

        protected async Task<HttpResponseMessage> Signatures(string ChannelName, string ChannelToken, string RequestorID, int WFID, string Profilename, int RecId, int verId, string SoapResult, string Activity, string Signature)
        {

            try
            {

                var Outfolder = System.AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", "/") + "Output";
                if (!Directory.Exists(Outfolder))
                {
                    Directory.CreateDirectory(Outfolder);
                }

                try
                {
                    var input = XDocument.Parse(SoapResult);
                    if (SoapResult.Contains("html"))
                    {
                        throw new Exception("Docuflo " + ResponseStatus.Error.ToString());
                    }

                    var uri = input.Descendants().ToList().Where(a => a.Name.LocalName == "FileURL").FirstOrDefault()?.Value;
                    var Fname = input.Descendants().ToList().Where(a => a.Name.LocalName == "FileName").FirstOrDefault()?.Value;
                    string[] FileNameCurrent = input.Descendants().ToList().Where(a => a.Name.LocalName == "FileName").FirstOrDefault()?.Value.Split(new Char[] { '.' });
                    var FNameNew = FileNameCurrent.FirstOrDefault()+"-Signed" +"." + FileNameCurrent.LastOrDefault();

                    var FullFname = Outfolder + "/" + Fname;

                    if (uri != null)
                    {
                        WebClient myWebClient = new WebClient();
                        myWebClient.DownloadFile(uri, FullFname);
                        myWebClient.Dispose();

                        var ResultSignature = MergeSignaturePDFAsync(FullFname, Signature, Outfolder.ToString(), Fname, Activity);

                        if (ResultSignature == "" || ResultSignature == "Error")
                        {
                            logger.Error("ProsesCheckout line 197");
                        }
                        else
                        {
                            logger.Error("Sukses Line 204");
                        }

                        var ResultCheckout = await ProcessCheckOut(ChannelName, ChannelToken, RequestorID, Profilename, RecId, verId);

                        ResultCheckout = ResultCheckout.Replace("\r\n", string.Empty);

                        if (ResultCheckout.ToString() != "Completed")
                        {
                            throw new Exception(ResultCheckout.ToString());
                            logger.Error(ResultCheckout.ToString() + " Error ProsesCheckout line 265");
                        }
                        else
                        {
                            logger.Error(ResultCheckout + " Line 269");
                        }

                        var ResultCheckin = ProcessCheckin(ChannelName, ChannelToken, RequestorID, WFID, Profilename, FNameNew, ResultSignature, RecId, verId);//Fname, ResultSignature, RecId, verId);

                        if (ResultCheckin.Result == "Error")
                        {
                            throw new Exception(ResultCheckin.Result.ToString());
                        }

                        var ResultVersioning = GetVersioning(ChannelName, ChannelToken, WFID, RequestorID, Profilename, FNameNew, RecId);

                        return Request.CreateResponse(HttpStatusCode.OK, ResultVersioning.Result.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        var Message = input.Descendants().ToList().Where(a => a.Name.LocalName == "ErrMsg").FirstOrDefault()?.Value;
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, Message);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("DL : " + ex.Message);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw ex;
            }
        }

        protected string MergeSignaturePDFAsync(string pdfFilePath, string base64File, string outputPath, string FileName, string Activity)
        {
            string FileNameOutput = pdfFilePath.Split('/').ToList().LastOrDefault().Split('.').First() + "-Signed.pdf";
            string outputsigned = outputPath + '/' + FileNameOutput;

            CUtility cs = new CUtility();
            string convert = base64File.Replace("data:image/png;base64,", String.Empty);
            convert = convert.Replace(" ", "+");
            System.Drawing.Image pfxFilePath = cs.LoadImage(convert);
            Stream input, output;//, pfx;
            MemoryStream OutMs = new MemoryStream();
            try
            {
                input = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);


                output = new FileStream(outputsigned, FileMode.Create, FileAccess.Write, FileShare.None);

                var putoncanvas = cs.PutOnCanvas(pfxFilePath, 300, 200, System.Drawing.Color.White);
                System.Drawing.Image img;

                using (var ms = new MemoryStream(putoncanvas))
                {
                    img = System.Drawing.Image.FromStream(ms);
                }

                System.Drawing.Image returnvoid;
                if (Activity == "Task Legal Manager - Perjanjian Kerjasama" || Activity == "Task Legal Manager -PSM")
                {
                    returnvoid = cs.Resize(img, 200, 100, false);
                    input.Position = 0;
                    PdfReader readers = new PdfReader(input);
                    PdfStamper stamper = new PdfStamper(readers, output);
                    iTextSharp.text.Image imgstamp = iTextSharp.text.Image.GetInstance(returnvoid, System.Drawing.Imaging.ImageFormat.Png);
                    imgstamp.SetAbsolutePosition(290, 60);
                    PdfContentByte stampimage;
                    for (int pageIndex = 1; pageIndex <= readers.NumberOfPages; pageIndex++)
                    {
                        stampimage = stamper.GetOverContent(pageIndex);
                        stampimage.AddImage(imgstamp);
                    }
                    stamper.FormFlattening = true;
                    OutMs.CopyTo(output);

                    stamper.Close();

                    
                }
                else if (Activity.Contains("CARO"))
                {
                    returnvoid = cs.Resize(img, 400, 200, false);
                    input.Position = 0;
                    PdfReader readers = new PdfReader(input);
                    PdfStamper stamper = new PdfStamper(readers, output);
                    iTextSharp.text.Image imgstamp = iTextSharp.text.Image.GetInstance(returnvoid, System.Drawing.Imaging.ImageFormat.Png);
                    imgstamp.SetAbsolutePosition(290, 60);
                    PdfContentByte stampimage;
                    for (int pageIndex = 1; pageIndex <= readers.NumberOfPages; pageIndex++)
                    {
                        stampimage = stamper.GetOverContent(pageIndex);
                        stampimage.AddImage(imgstamp);
                    }
                    stamper.FormFlattening = true;
                    OutMs.CopyTo(output);

                    stamper.Close();
                }


                input.Close();
                var Base64 = GenerateBase64(outputsigned);
                output.Close();

                //var Base64 = ConvertToBase64(output);
                logger.Error(" line 350 base64");

                return Base64;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";
            }
        }

        protected string GenerateBase64(string path)
        {
            try
            {
                string filename = path;
                FileStream f = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                int size = (int)f.Length;
                byte[] MyData = new byte[f.Length + 1];
                f.Read(MyData, 0, size);
                f.Close();
                return Convert.ToBase64String(MyData);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        protected async Task<string> ProcessCheckin(string ChannelName, string ChannelToken, string RequestorID, int WFID, string Profilename, string FileName, string Base64, int RecId, int verId)
        {
            CUtility cs = new CUtility();
            RestClient client = new(cs.DocuFloCheckInDoc);
            //RestClient client = new("https://ecm-uat-dms.eigerindo.co.id/Docuflo7_WebApi/Docuflo7_WebApi.asmx?op=CheckInDoc");

            try
            {
                string ModuleCode = "";
                if (Profilename.Contains("ECM - 05 Legal"))
                {
                    ModuleCode = "05";
                }

                Repository rp = new Repository();
                DynamicParameters dParam = new DynamicParameters();
                dParam.Add("@profileName", Profilename, DbType.String, ParameterDirection.Input);
                dParam.Add("@WorkflowId", WFID, DbType.Int64, ParameterDirection.Input);
                dParam.Add("@ModuleCode", ModuleCode, DbType.String, ParameterDirection.Input);

                var ProfileFields = (rp.DynamicParamQuery<ProfileFieldDMS>("[precise.GenerateProfileFieldDMS]", dParam, null, true, CommandType.StoredProcedure)).List.ToList();
                logger.Error(ProfileFields + " Line 393");

                RestRequest request = new()
                {
                    Method = Method.POST
                };
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");

                var body = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <soap:Header>
                            <Authentication xmlns=""http://tempuri.org/"">
                              <ChannelName>" + ChannelName + @"</ChannelName>
                              <ChannelToken>" + ChannelToken + @"</ChannelToken>
                            </Authentication>
                        </soap:Header>
                        <soap:Body>
                            <CheckInDoc xmlns=""http://tempuri.org/"">
                              <RequestorID>" + RequestorID + @"</RequestorID>
                              <RecID>" + RecId + @"</RecID>
                              <VerID>" + verId + @"</VerID>  
                              <ClassID>9</ClassID>
                              <ProfileName>" + Profilename + @"</ProfileName>
                              <Indexes>
                                <IndexItem>
                                  <Name>string</Name>
                                  <Description>string</Description>
                                  <Value>string</Value>
                                </IndexItem>
                              </Indexes>
                              <FileName>" + FileName.Replace("&","and") + @"</FileName>
                              <FileContent>" + Base64 + @"</FileContent>
                              <ErrCode></ErrCode>
                              <ErrMsg></ErrMsg>
                            </CheckInDoc>
                        </soap:Body>
                        </soap:Envelope>";

                string covert = LoopingXML(body, ProfileFields);
                logger.Error(covert + " Line 487");

                request.AddParameter("text/xml; charset=utf-8", covert, ParameterType.RequestBody);
                var response = client.ExecuteAsPost(request, request.Method.ToString());

                if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    throw new Exception(response.ResponseStatus.ToString());
                    logger.Error(response.ResponseStatus.ToString() + " Line 494");
                }
                else
                {
                    logger.Error(response.ResponseStatus.ToString() + " Line 498");
                }
                return response.ResponseStatus.ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new Exception("may not use the characters ',&,<,>,/ in this document");
                //Request.Response(new ApiStatus(500, "may not use the characters ',&,<,>,/ in this document"), null, "Error");
            }
        }

        public string LoopingXML(string xmltext, List<ProfileFieldDMS> data)
        {
            var xml = "";
            XmlDocument _doc = new XmlDocument();

            //XDocument _doc = XDocument.Parse(xmltext);
            //logger.Error(data.Count + "Line 513");
            //logger.Error(xmltext + "Line 514");

            try
            {
                //_doc.Load(xmltext);

                _doc.LoadXml(xmltext);
                var adoc = _doc.DocumentElement.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(5);

                for (int i = 0; i < data.Count; i++)
                {
                    if (i == 0)
                    {
                       
                        adoc.ChildNodes.Item(0).ChildNodes.Item(0).InnerText = data[i].FieldName;
                        adoc.ChildNodes.Item(0).ChildNodes.Item(1).InnerText = data[i].Descriptions;
                        adoc.ChildNodes.Item(0).ChildNodes.Item(2).InnerText = data[i].Values;

                    }
                    else
                    {
                        XmlNode nodeItem = _doc.CreateElement("IndexItem","");
                        adoc.AppendChild(nodeItem);

                        XmlNode nodename = _doc.CreateElement("Name");
                        nodename.InnerText= data[i].FieldName;
                        nodeItem.AppendChild(nodename);

                        XmlNode nodedesc = _doc.CreateElement("Description");
                        nodedesc.InnerText = data[i].Descriptions;
                        nodeItem.AppendChild(nodedesc);

                        XmlNode nodeValue = _doc.CreateElement("Value");
                        nodeValue.InnerText = data[i].Values;
                        nodeItem.AppendChild(nodeValue);
                    }
                }
                xml = _doc.InnerXml.ToString();
                logger.Error(xml + " Line 538");
                return xml;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString);
                
                return "";
            }
        }

        protected async Task<string> ProcessCheckOut(string ChannelName, string ChannelToken, string RequestorID, string Profilename, int RecId, int verId)
        {
            CUtility cs = new CUtility();
            //RestClient client = new("https://ecm-prod-dms.eigerindo.co.id/Docuflo7_WebApi/Docuflo7_WebApi.asmx?op=CheckOutDoc");
            RestClient client = new(new CUtility().DocuFloCheckOutDoc);

            try
            {
                RestRequest request = new()
                {
                    Method = Method.POST
                };
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");

                var body = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                           <soap:Header>
                            <Authentication xmlns=""http://tempuri.org/"">
                              <ChannelName>" + ChannelName + @"</ChannelName>
                              <ChannelToken>" + ChannelToken + @"</ChannelToken>
                            </Authentication>
                          </soap:Header>
                          <soap:Body>
                            <CheckOutDoc xmlns=""http://tempuri.org/"">
                                <reqData>
                                    <RequestorID>" + RequestorID + @"</RequestorID>
                                    <ProfileName>" + Profilename + @"</ProfileName>
                                    <RecID>" + RecId + @"</RecID>
                                    <VerID>" + verId + @"</VerID>
                                </reqData>
                            </DownloadDoc>
                          </soap:Body>
                         </soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
                var response = client.ExecuteAsPost(request, request.Method.ToString());
                if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    throw new Exception(response.ResponseStatus.ToString());
                    logger.Error("Line 473");
                }

                logger.Error(response.ResponseStatus.ToString() + "Line 471");
                return response.ResponseStatus.ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "Error Line 481");
                return ex.Message;//Request.Response(new ApiStatus(500, ex.Message), null, "Error");
            }
        }

        //protected IHttpActionResult GetVersioning(string ChannelName, string ChannelToken, int WFID, string RequestorID, string Profilename, int RecId)
        //{
        //    try
        //    {
        //        RestClient client = new(new CUtility().DocuFloVersionDoc);

        //        RestRequest request = new()
        //        {
        //            Method = Method.POST
        //        };
        //        request.AddHeader("Content-Type", "text/xml");
        //        var body = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
        //                          <soap:Header>
        //                            <Authentication xmlns=""http://tempuri.org/"">
        //                                <ChannelName>" + ChannelName + @"</ChannelName>
        //                                <ChannelToken>" + ChannelToken + @"</ChannelToken>
        //                            </Authentication>
        //                          </soap:Header>
        //                          <soap:Body>
        //                            <GetDocVersioning xmlns=""http://tempuri.org/"">
        //                              <reqData>
        //                                <RequestorID>" + RequestorID + @"</RequestorID>
        //                                <ProfileName>" + Profilename + @"</ProfileName> 
        //                                <RecID>" + RecId + @"</RecID>
        //                              </reqData>
        //                            </GetDocVersioning>
        //                          </soap:Body>
        //                        </soap:Envelope>";
        //        request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
        //        var response = client.ExecuteAsPost(request, request.Method.ToString());

        //        if (response.ResponseStatus != ResponseStatus.Completed)
        //        {
        //            var ResultsError = new
        //            {
        //                ProcessSuccess = false,
        //                InfoMessage = response.ResponseStatus
        //            };
        //            return Json(ResultsError);

        //            //return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Status = ResponseStatus.Error, Message = response.ResponseStatus.ToString() });
        //            logger.Error(response.ResponseStatus.ToString() + " Line 161");
        //        }

        //        if (response.Content.Contains("html"))
        //        {
        //            var ResultsHtml = new
        //            {
        //                ProcessSuccess = false,
        //                InfoMessage = response.ResponseStatus
        //            };
        //            return Json(ResultsHtml);
        //            //return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Status = ResponseStatus.Error, Message = "Internal Server Error" });
        //            //throw new Exception(ResponseStatus.Error.ToString());
        //        }

        //        XDocument doc = XDocument.Parse(response.Content);
        //        List<(Int64 nodes, string verno, string verid)> lists = new();
        //        string keytosearch = "lstDocVerInfo";
        //        var firstdegree = doc.Descendants().Where(a => a.Name.LocalName.ToLower() == keytosearch.ToLower()).Elements().ToList();
        //        long i = 0;
        //        firstdegree.ForEach(a => {
        //            var vid = a.Descendants().Where(aa => aa.Name.LocalName == "VerID").FirstOrDefault().Value;
        //            var vno = a.Descendants().Where(aa => aa.Name.LocalName == "VerNo").FirstOrDefault().Value;
        //            lists.Add(new() { nodes = i, verid = vid, verno = vno });
        //            i++;
        //        });

        //        var verid = Convert.ToInt32(lists[lists.Count - 1].verid);
        //        if (verid < 1) { throw new Exception("verid not valid"); }

        //        Repository rp = new Repository();
        //        DynamicParameters dParam = new DynamicParameters();
        //        dParam.Add("@WorkflowId", WFID, DbType.Int64, ParameterDirection.Input);
        //        dParam.Add("@Verid", verid, DbType.Int32, ParameterDirection.Input);
        //        dParam.Add("@RecId", RecId, DbType.Int32, ParameterDirection.Input);

        //        var InsertData = (rp.DynamicParamQuery<Int32>("dbo.[precise.05.SP_DMSUploadInsert]", dParam, null, true, CommandType.StoredProcedure));

        //        var Results = new
        //        {
        //            ProcessSuccess = true,
        //            InfoMessage = "Sukses"
        //        };

        //        return Json(Results);
        //    }
        //    catch (Exception ex)
        //    {
        //        var Results = new
        //        {
        //            ProcessSuccess = false,
        //            InfoMessage = ex.Message
        //        };
        //        return Json(Results);
        //    }
        //}

        protected async Task<HttpResponseMessage> GetVersioning(string ChannelName, string ChannelToken, int WFID, string RequestorID, string Profilename,string FileNameNew, int RecId)
        {
            try
            {

                //RestClient client = new("https://ecm-uat-dms.eigerindo.co.id/Docuflo7_WebApi/Docuflo7_WebApi.asmx?op=GetDocVersioning");
                RestClient client = new(new CUtility().DocuFloVersionDoc);

                RestRequest request = new()
                {
                    Method = Method.POST
                };
                request.AddHeader("Content-Type", "text/xml");
                var body = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                  <soap:Header>
                                    <Authentication xmlns=""http://tempuri.org/"">
                                        <ChannelName>" + ChannelName + @"</ChannelName>
                                        <ChannelToken>" + ChannelToken + @"</ChannelToken>
                                    </Authentication>
                                  </soap:Header>
                                  <soap:Body>
                                    <GetDocVersioning xmlns=""http://tempuri.org/"">
                                      <reqData>
                                        <RequestorID>" + RequestorID + @"</RequestorID>
                                        <ProfileName>" + Profilename + @"</ProfileName> 
                                        <RecID>" + RecId + @"</RecID>
                                      </reqData>
                                    </GetDocVersioning>
                                  </soap:Body>
                                </soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
                var response = client.ExecuteAsPost(request, request.Method.ToString());

                if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Status = ResponseStatus.Error, Message = response.ResponseStatus.ToString() });
                    //throw new Exception(response.ResponseStatus.ToString());
                    logger.Error(response.ResponseStatus.ToString() + " Line 161");
                }

                if (response.Content.Contains("html"))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Status = ResponseStatus.Error, Message = "Internal Server Error" });
                    //throw new Exception(ResponseStatus.Error.ToString());
                }

                XDocument doc = XDocument.Parse(response.Content);
                List<(Int64 nodes, string verno, string verid)> lists = new();
                string keytosearch = "lstDocVerInfo";
                var firstdegree = doc.Descendants().Where(a => a.Name.LocalName.ToLower() == keytosearch.ToLower()).Elements().ToList();
                long i = 0;
                firstdegree.ForEach(a =>
                {
                    var vid = a.Descendants().Where(aa => aa.Name.LocalName == "VerID").FirstOrDefault().Value;
                    var vno = a.Descendants().Where(aa => aa.Name.LocalName == "VerNo").FirstOrDefault().Value;
                    lists.Add(new() { nodes = i, verid = vid, verno = vno });
                    i++;
                });

                var verid = Convert.ToInt32(lists[lists.Count - 1].verid);
                if (verid < 1) { throw new Exception("verid not valid"); }

                Repository rp = new Repository();
                DynamicParameters dParam = new DynamicParameters();
                dParam.Add("@WorkflowId", WFID, DbType.Int64, ParameterDirection.Input);
                dParam.Add("@Verid", verid, DbType.Int32, ParameterDirection.Input);
                dParam.Add("@RecId", RecId, DbType.Int32, ParameterDirection.Input);
                dParam.Add("@FileName", FileNameNew, DbType.String, ParameterDirection.Input);

                var InsertData = (rp.DynamicParamQuery<Int32>("dbo.[precise.05.SP_DMSUploadInsert]", dParam, null, true, CommandType.StoredProcedure));

                string status = response.ResponseStatus.ToString();

                return Request.CreateResponse(HttpStatusCode.OK, new { Status = status, Message = "" });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Status = ResponseStatus.Error, Message = ex.Message.ToString() });

            }
        }

        [HttpPost]
        [Route("PostDownload")]
        public IHttpActionResult PostDownload(string ChannelName, string ChannelToken, string RequestorID, int WFID, string Profilename, int RecId, int verId, string Activity)
        {
            try
            {
                ProductService ps = new ProductService();
                string Signature = ps.GetSignatureDB(WFID, Activity);

                var retres = GetSoapFrom(ChannelName, ChannelToken, RequestorID, WFID, Profilename, RecId, verId, Activity, Signature);

                var ResultReason = retres.Result.ReasonPhrase;
                //var des = (List<ResultGen>)retres;
                if (ResultReason != "OK")
                {
                    var ResultsReason = new
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        InfoMessage = ResultReason
                    };
                    return Json(ResultsReason);
                }


                var Results = new
                {
                    StatusCode = HttpStatusCode.OK,
                    InfoMessage = "Sukses"
                };
                return Json(Results);
            }
            catch (Exception ex)
            {
                var Results = new
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    InfoMessage = ex.Message
                };
                return Json(Results);
            }
        }

            //[HttpPost]
            ////[ResponseType(typeof(IEnumerable<GenerateSignatures>))]
            //[Route("PostDownload")]
            //public async Task<HttpResponseMessage> PostDownload(string ChannelName, string ChannelToken, string RequestorID, int WFID, string Profilename, int RecId, int verId, string Activity)
            //{
            //    GenerateSignatures results = new GenerateSignatures();
            //    try
            //    {
            //        ProductService ps = new ProductService();
            //        string Signature = ps.GetSignatureDB(WFID, Activity);

            //        var retres = await GetSoapFrom(ChannelName, ChannelToken, RequestorID, WFID, Profilename, RecId, verId, Activity,Signature);


            //        results.Results.FirstOrDefault().Status = retres.StatusCode.ToString();
            //        var Results = JsonConvert.DeserializeObject<GenerateSignatures>(retres.Content.ReadAsStringAsync().Result).Results.FirstOrDefault().Message;
            //        results.Results.FirstOrDefault().Message = Results;//await retres.Content.ReadAsStringAsync();

            //        return Request.CreateResponse(HttpStatusCode.OK, new { results.Status, System.Text.Json.JsonSerializer.Deserialize<GenerateSignatures>(Results).Results.FirstOrDefault().Message });//results.Message });
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error(ex);
            //        results.Results.FirstOrDefault().Status = ResponseStatus.Error.ToString();
            //        results.Results.FirstOrDefault().Message = ex.Message.ToString();

            //        return Request.CreateResponse(HttpStatusCode.OK, new { results.Results.FirstOrDefault().Status, results.Results.FirstOrDefault().Message });
            //    }
            //}

        }
}