using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueTierLevels
{
    class ApiErrorHandler
    {
        private static int rateLimitHits = 0;
        private static int serverErrors = 0;



        public bool Handle(Exception ex)
        {
            //get html code
            string message = ex.Message;
            string[] splitMessage = message.Split(new string[] { "status:" }, StringSplitOptions.None);
            message = splitMessage[1].Remove(4);
            int code = Convert.ToInt32(message);

            //get first digit
            int firstDigit = Math.Abs(code);
            while (firstDigit >= 10)
            {
                firstDigit /= 10;
            }

            bool successfullyHandled;

            switch (code)
            {
                case 400:
                    successfullyHandled = BadRequest();
                    break;
                case 401:
                    successfullyHandled = Unauthorized();
                    break;
                case 403:
                    successfullyHandled = Forbidden();
                    break;
                case 404:
                    successfullyHandled = NotFound();
                    break;
                case 415:
                    successfullyHandled = UnsupportedMediaType();
                    break;
                case 429:
                    successfullyHandled = RateLimitExceeded();
                    break;
                case 500:
                    successfullyHandled = InternalServerError();
                    break;
                case 503:
                    successfullyHandled = ServiceUnavailable();
                    break;
                default:
                    successfullyHandled = false;
                    break;
            }
            return successfullyHandled;
        }
   

        private bool BadRequest()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("A BAD REQUEST WAS SUBMITTED. AN INPUT FIELD IS INCORRECT");
            Console.ResetColor();
            return false;
        }

        //Endpoints are declared via camille an unathorized request is a problem with camille
        private bool Unauthorized()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("YOU ARE UNAUTHERIZED TO ACCESS THIS ENDPOINT");
            Console.ResetColor();
            return false;
        }

        private bool Forbidden()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("API KEY IS INVALID");
            Console.ResetColor();
            return false;
        }

        private bool NotFound()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("A SUMMONER COULD NOT BE FOUND");
            Console.ResetColor();
            return true;
        }

        //Again created via camille
        private bool UnsupportedMediaType()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("INVALID MEDIA TYPE");
            Console.ResetColor();
            return false;
        }

        //user most likely specified production key but provided a personal/dev key
        private bool RateLimitExceeded()
        {
            rateLimitHits++;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("RATE LIMIT EXCEEDED");
            Console.ResetColor();

            Api api = Api.ApiInstance;
            if (api.keyType != ApiType.PRODUCTION)
            {
                Console.WriteLine("SLOWING DOWN RATE");
                api.ChangeKeyType(ApiType.DEFAULT);
                return true;
            }
            else
            {
                if (rateLimitHits > 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CONSISTANTLY HITTING RATE LIMIT");
                    Console.WriteLine("WILL NOT ATTEMPT TO MAKE ANY MORE CALLS");
                    Console.WriteLine("MAKE SURE THE CORRECT KEY TYPE IS USED");
                    Console.ResetColor();
                    return false;
                }

                int pauseTime = 30;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ALREADY AT SLOWEST RATE");
                Console.WriteLine("PAUSING API CALLS FOR {0} SECONDS", pauseTime);
                Console.ResetColor();

                api.rateLimiter.Pause(pauseTime);
                return true;
            }
        }

        private bool InternalServerError()
        {
            serverErrors++;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("INTERNAL SERVER ERROR");
            Console.ResetColor();

            if (serverErrors > 10)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SERVER FAILS CONSISTANTLY");
                Console.WriteLine("STOPPING CALLS");
                Console.ResetColor();
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ATTEMPTING CALL AGAIN");
                Console.ResetColor();
                return true;
            }
        }

        private bool ServiceUnavailable()
        {
            serverErrors++;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("SERVICE UNAVAILABLE");
            Console.ResetColor();

            if (serverErrors > 10)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SERVER FAILS CONSISTANTLY");
                Console.WriteLine("STOPPING CALLS");
                Console.ResetColor();
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ATTEMPTING CALL AGAIN");
                Console.ResetColor();
                return true;
            }
        }


    }
}
