﻿using Twilio.TwiML.Messaging;
using Twilio.TwiML.Voice;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Hangfire;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Task = System.Threading.Tasks.Task;

namespace HospitalManagement.Jobs
{
    public class BackgroundJobs
    {        

        public static void NotificationForPatient(string name, string contactNo,DateTime appointmentDate )
        {
            BackgroundJob.Enqueue(() => SendNotification(contactNo
                    , $"Hello {name}! your appointment on {appointmentDate} is cancelled due to doctor's personal emergency. Sorry for the inconvinience and Kindly, Reschedule you appointment later.\nThank you"));
        }

        public static void NotificationForDoctor(string name, string contactNo, DateTime appointmentDate,TimeOnly slot)
        {
            BackgroundJob.Enqueue(() => SendNotification(contactNo
                    , $"Hello Dr.{name}! You have a new appointment on {appointmentDate} , {slot} slot.\nThank you"));
        }

        public static void CancelNotificationForDoctor(string name, string contactNo, DateTime appointmentDate, TimeOnly slot)
        {
            BackgroundJob.Enqueue(() => SendNotification(contactNo
                    , $"Hello Dr.{name}! your appointment on {appointmentDate} , {slot} slot is cancelled.\nThank you"));
        }

        public static async Task SendNotification(string phoneNumber,string message)
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

            var keyVaultUri = "https://pavihoskeyvault.vault.azure.net/";
            var keyVaultClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

            var twilioSettings = configuration.GetSection("Twilio").Get<Twilio>();
            var accountSid = await keyVaultClient.GetSecretAsync("AccountSid");
            var authToken = await keyVaultClient.GetSecretAsync("AuthToken");

            TwilioClient.Init(accountSid.Value.Value, authToken.Value.Value);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
            {
                From = new PhoneNumber(twilioSettings.PhoneNo),
                Body = message
            };

            MessageResource.Create(messageOptions);
        }

        public  async static Task SendOTPToPatient(string phoneNumber, int otp, string name)
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

            var keyVaultUri = "https://pavihoskeyvault.vault.azure.net/";
            var keyVaultClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

            var twilioSettings = configuration.GetSection("Twilio").Get<Twilio>();
            var accountSid = await keyVaultClient.GetSecretAsync("AccountSid");
            var authToken = await keyVaultClient.GetSecretAsync("AuthToken");

            TwilioClient.Init(accountSid.Value.Value, authToken.Value.Value);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
            {
                From = new PhoneNumber(twilioSettings.PhoneNo),
                Body = $"Hi {name}! your One time password for logging into Hospital management system is {otp}."
            };

            var sentMessage = MessageResource.Create(messageOptions);
        }

        public static string GenerateOTPAndSend(string contactNo, string name)
        {
            Random random = new Random();
            int otp = random.Next(10000, 100000);
            BackgroundJob.Enqueue(() =>SendOTPToPatient(contactNo, otp, name));
            return otp.ToString();
        }
    }
}
