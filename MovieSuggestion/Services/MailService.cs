using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MovieSuggestion.Models.Entities.View;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Threading.Tasks;
using MovieSuggestion.Models;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MovieSuggestion.Services
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailVM mailModel);
    }

    public class MailService : IMailService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public MailService(ApplicationDbContext db,
                           IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        private IConfigurationRoot _config
        {
            get
            {
                return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            }
        }

        public async Task<bool> SendAsync(MailVM mailModel)
        {
            var movie = await _mapper.ProjectTo<MovieGetModel>(_db.Movie.AsNoTracking().Where(x => x.Id == mailModel.MovieId)).SingleOrDefaultAsync();

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_config["SmtpConfig:Name"], _config["SmtpConfig:EmailAddress"]));
            message.To.Add(new MailboxAddress(mailModel.ToAddressName, mailModel.ToAddress));

            message.Subject = $"'{movie.Name}' bu filmi mutlaka izlemelisin!";

            string moviePath = "https://image.tmdb.org/t/p/original";

            string body = "";
            body += $"<img src='{moviePath}{movie.Image}' style='width:250px' /><br />";
            body += $"<strong>Film Adı&nbsp;:</strong> {movie.Name}<br />";
            body += $"<strong>Puanı&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</strong> {movie.AvgRate}<br /><br />";
            body += "İyi seyirler...";

            var builder = new BodyBuilder { HtmlBody = body };

            message.Body = builder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    if (!Convert.ToBoolean(_config["SmtpConfig:UseSSL"]))
                        client.ServerCertificateValidationCallback = (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

                    await client.ConnectAsync(_config["SmtpConfig:Host"], Convert.ToInt32(_config["SmtpConfig:Port"]), Convert.ToBoolean(_config["SmtpConfig:UseSSL"])).ConfigureAwait(false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    if (!string.IsNullOrWhiteSpace(_config["SmtpConfig:Username"]))
                        await client.AuthenticateAsync(_config["SmtpConfig:Username"], _config["SmtpConfig:Password"]).ConfigureAwait(false);

                    await client.SendAsync(message).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
