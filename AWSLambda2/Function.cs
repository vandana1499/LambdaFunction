using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambda2
{
    public class Person
    {
        public string fName { get; set; }
        public string eAddress { get; set; }
        public string strState { get; set; }
        public string firmSize { get; set; }
        public string website { get; set; }
        public string firmName { get; set; }
        public string expertise { get; set; }
        public override string ToString()
        {
            return base.ToString() + ":" + fName.ToString() + " " + eAddress.ToString() + " " + strState.ToString() + " " + firmSize.ToString()+ " " + website.ToString() + " " +
                firmName.ToString() + " " + expertise.ToString();
        }

    }
    public class Function
    {

        public const string INVOCATION_NAME = "the legalzoom";
        public string[] values=new string[7];
        public int counter = 0;
        public string[] arr = {"name", "Email", "Firm Name", "Website", "Law firm size", "states licensed", "practice areas" };
        private string[] response = new string[7];
        Person obj = new Person();
      

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {

            Session session = input.Session;
            if (session.Attributes == null)
                session.Attributes = new Dictionary<string, object>();

            if (input.GetRequestType() == typeof(LaunchRequest))
            {

                LambdaLogger.Log("Lmbda has started");
                string speech = "Welcome to legalzoom";

                return MakeSkillResponse(speech, true);
            }
            else if (input.GetRequestType() == typeof(SessionEndedRequest))
            {
                return ResponseBuilder.Tell("Goodbye!");
            }
          else  if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = input.Request as IntentRequest;
                var outputText="";
                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                    case "AMAZON.StopIntent":
                        return ResponseBuilder.Tell("Goodbye!");
                      
                    case "AMAZON.HelpIntent":
                        {
                         
                            return ResponseBuilder.Tell("Here's some help. What's next?");
                        }
                        
                    case "informationLegalzoom":
                        {

                            LambdaLogger.Log("Lmbda has started");
                         
                            string speech = "Welcome ! LegalZoom.com, Inc. is an online legal " +
                                "technology company that helps its customers create legal documents without necessarily having to hire a lawyer.";
                            Reprompt rp = new Reprompt("Please ask your queries");
                            return ResponseBuilder.Ask(speech, rp, session);
                        }
                        
                    case "GetPriceOfJoinAttorney":
                        {
                            var product = intentRequest.Intent.Slots["product"].Value;
                            if (product == null)
                            {
                                context.Logger.LogLine($"The product was not understood.");
                                return MakeSkillResponse("I'm sorry, but I didn't understand the product you were asking for. Please ask again.", false);
                            }
                            outputText = $"You'd like more information about the price of {product}.To get the information about it in Legalzoom";
                            return MakeSkillResponse(outputText, true);
                        }
                    case "joinattorneynetwork":
                        {

                           
                            string speech = "No worries,we will help u with that ." +
                                "You need to answer questions that are asked ." +
                                "What is your name?";
                            counter = counter + 1;
                            Reprompt rp = new Reprompt(speech);
                             return ResponseBuilder.Ask(speech, rp, session);

                        }
                    case "answer":
                        {
                            string speech3 = "";
                            Reprompt rp3;
                            string val = intentRequest.Intent.Slots["value"].Value;
                          
                          
                            if (counter > 6)
                            {
                                LambdaLogger.Log("Questionnsss completed...Now call to httpclient");
                                obj.expertise = intentRequest.Intent.Slots["value"].Value;
                             
                                string speech2 = "Practice area as "+val+" seems awesome .Thanks for the info .You will soon receive an email from us" ;
                                Reprompt rp1 = new Reprompt(speech2);
                                try
                                {
                                    LambdaLogger.Log("here entering the try block of counter >6");
                                    LambdaLogger.Log("Person-:" + obj.ToString());
                                    LambdaLogger.Log(FunctionHandler1(obj).Result);
                                    counter = 0;
                                }
                                catch(Exception e)

                                {

                                    LambdaLogger.Log("Exception:"+e+"\n inner exception:"+e.InnerException);
                                }
                                return ResponseBuilder.Ask(speech2, rp1, session);
                            }
                            else
                            {
                              
                               
                                if(counter==1)
                                {
                                    obj.fName = val;
                                    speech3 += "Hi "+obj.fName+" ! Hope you are doing well . What is your " + GetQuestion(counter)+" Address";
                                    counter = counter + 1;
                                    rp3 = new Reprompt(speech3);

                                    return ResponseBuilder.Ask(speech3, rp3, session);
                                }
                                if(counter==2)
                                {
                                    obj.eAddress= intentRequest.Intent.Slots["value"].Value;
                                    //int countSpaces = email.Count(Char.IsWhiteSpace);
                                    //if(countSpaces>1)
                                    //{
                                    //    speech3 += "Hi "+obj.fName+" ! Sorry you entered wrong email id .Please tell it once more";
                                    //    rp3 = new Reprompt(speech3);
                                    //    return ResponseBuilder.Ask(speech3, rp3, session);

                                    //}
                                    //else
                                    //{
                                    //    int i = email.IndexOf(" ");
                                    //    email.Insert(i, "@");
                                       
                                    //}
                                    //if(!isValidEmail(email))
                                    //{
                                    //    speech3 += "Hi " + obj.fName + " ! Sorry you entered wrong email id .Please tell it once more";
                                    //    rp3 = new Reprompt(speech3);
                                    //    return ResponseBuilder.Ask(speech3, rp3, session);
                                    //}
                                    obj.eAddress= intentRequest.Intent.Slots["value"].Value;
                                    speech3 += obj.fName +", please tell us your " + GetQuestion(counter) ;
                                    counter = counter + 1;
                                    rp3 = new Reprompt(speech3);
                                    return ResponseBuilder.Ask(speech3, rp3, session);
                                }
                                if (counter == 3)
                                {
                                    obj.firmName= intentRequest.Intent.Slots["value"].Value;
                                    speech3 += "WOW "+obj.fName+", name of your firm is beautifuly kept . Please help us by telling your " + GetQuestion(counter);
                                    counter = counter + 1;
                                    rp3 = new Reprompt(speech3);

                                    return ResponseBuilder.Ask(speech3, rp3, session);
                                }
                                if (counter == 4)
                                {
                                    obj.website = intentRequest.Intent.Slots["value"].Value;
                                    speech3 += "We will definitely look your website. Please tell us what is the " + GetQuestion(counter);
                                    counter = counter + 1;
                                    rp3 = new Reprompt(speech3);
                                    return ResponseBuilder.Ask(speech3, rp3, session);
                                }
                                if (counter == 5)
                                {
                                    obj.firmSize= intentRequest.Intent.Slots["value"].Value;
                                    speech3 += "Thats nice! Please tell us " + GetQuestion(counter);
                                    counter = counter + 1;
                                    rp3 = new Reprompt(speech3);
                                    return ResponseBuilder.Ask(speech3, rp3, session);
                                }

                                obj.strState = intentRequest.Intent.Slots["value"].Value;
                                speech3 += "Wow "+val+" is a beautiful place .We will surely visit someday .Please tell us your" + GetQuestion(counter);
                                counter = counter + 1;
                                rp3 = new Reprompt(speech3);
                                    return ResponseBuilder.Ask(speech3, rp3, session);
                                
                               
                            }
                        }



                    default:
                        {
                            string speech = "I didn't understand - try again?";
                            Reprompt rp = new Reprompt(speech);
                            return ResponseBuilder.Ask(speech, rp, session);
                        }
                        
                }

               
            }
            else
            {
                return MakeSkillResponse(
                        $"I don't know how to handle this intent.",
                        true);
            }
        }

        public static bool isValidEmail(string inputEmail)
        {
            LambdaLogger.Log(inputEmail);
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        public string GetQuestion(int counter)
        {
            
            return arr[counter];
        }


        public async Task<string> FunctionHandler1(Person input)
        {
            LambdaLogger.Log("Inside func handler1");

            try
            {
                LambdaLogger.Log(" try block ofInside func handler1");


                using (var client = new HttpClient())
                {

                    LambdaLogger.Log(" try block of Inside func handler1 inside using");
                    //jnnnhttps://jsonplaceholder.typicode.com/posts"
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://jsonplaceholder.typicode.com/posts");

                    request.Content = new StringContent(JsonConvert.SerializeObject(input));
                    var response = await client.SendAsync(request);
                    LambdaLogger.Log("Successsfully received response"+response);
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }

            }
            catch(Exception e)
            {

                Console.WriteLine("inside catch block of func handler"+e+"  "+e.Message+" "+e.InnerException);
                return "";
            }
           
        }
            private SkillResponse MakeSkillResponse(string outputSpeech,
           bool shouldEndSession)
           //string repromptText = "Just say, tell me about the price of LLC to learn more. To exit, say, exit.")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };


            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.0"
            };
            return skillResponse;
        }
    }
}
