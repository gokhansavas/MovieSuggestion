# MovieSuggestion
Film Önerileri | Film puanlayın, Film yorumlayın, Arkadaşlarınıza film tavsiyelerinde bulunun..


[MovieSuggestionDb.bak] Full backup'ını MS-SQL ortamına kurduktan sonra 2 adet demo kullanıcısı ve 2 adet film yorum/puanlaması gelecektir.
Film tablosu boşaltılmıştır, Job aracılığıyla TMDB üzerinden her saat 20 adet kayıt yapacaktır.
User ve MovieScore bilgilerini SQL'den silebilirsiniz, API'ler üzerinden kendiniz oluşturabilirsiniz.


Kullandığım teknolojiler:

- .NET Core (5.0)
- MS-SQL
- EntityFramework (Core)
- AutoMapper
- Quartz Job (Core)
- JWT Bearer Token
- MimeKit / MailKit
