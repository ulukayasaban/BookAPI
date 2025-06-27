# 📚 Book.API

ASP.NET Core 9.0 ile geliştirilmiş, SQLite veritabanı kullanan, JWT Authentication destekli ve CQRS mimarisiyle yapılandırılmış bir kitap yönetim API'sidir.

---

## 🚀 Özellikler

- ✅ ASP.NET Core 9.0
- ✅ CQRS + MediatR ile yapılandırılmış mimari
- ✅ Entity Framework Core (SQLite)
- ✅ AutoMapper ile DTO dönüşümleri
- ✅ JWT ile kullanıcı girişi
- ✅ Identity ile kullanıcı yönetimi
- ✅ Serilog ile gelişmiş loglama (Sadece Console'a)
- ✅ Global exception middleware
- ✅ Unit test: xUnit + Moq + FluentAssertions
- ✅ Kod kapsama (coverage) takibi

---

## 🔐 Identity Şifre Kuralları

Şifreler için kullanılan kurallar (Program.cs içinde tanımlı):

- Minimum uzunluk: **6 karakter**
- E-posta benzersiz olmalı
- Maksimum 5 başarısız deneme sonrası 5 dakika kilitlenme

---

## ⚙️ Kurulum Adımları

1. **Projeyi klonlayın:**
```bash
git clone https://github.com/sabanulukaya/Book.API.git
cd Book.API
```
2. **Bağımlılıkları yükleyin:**
```bash
dotnet restore
```
3. **Veritabanını oluşturun:**
```bash
dotnet ef database update
```
4. **Uygulamayı başlatın:**
```bash
dotnet run --project Book.API
```
Swagger UI → http://localhost:5011/swagger/index.html

##  🔐 JWT ile Giriş İşlemleri
-   `POST /api/users/register`  → Yeni kullanıcı kaydı
-   `POST /api/users/login` → JWT token alımı
JWT token, `Authorization` header'ı ile gönderilmelidir:
-   Kullanıcı girişinde token döndürülür.
-   Token, `appsettings.json` dosyasındaki `Secret` değeri ile HMAC-SHA256 algoritması kullanılarak imzalanır.##
##  ⚠️ JWT ayarları
`Program.cs` içinde tanımlanmıştır:
```csharp
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x => 
{
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidIssuer="sabanulukaya.com",
        ValidateAudience = false,
        ValidAudience="CompanyName",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("appsettings:Secret").Value ?? "")),

        ValidateLifetime = true 
    };
});
```
⚠️ JWT token oluşturulurken HmacSha256Signature (HS256) algoritması kullanılmaktadır.
Bu nedenle Secret değeri minimum 32 karakter uzunluğunda olmalıdır.
```json
"AppSettings":{
    "Secret":"9e15c25ddafd22f4d76f643f08caa9c12af528a29d6a4a530af279fc01823412ef3ae04b1df2b6d7309fb5b2253dfcc963612def0b38eb0c7298a911090e3e37"
  }
```
## 🧪 Testler ve Coverage
Proje için xUnit + FluentAssertions ile kapsamlı testler yazılmıştır.

### ✅ Test Edilen Alanlar:
-   CQRS Komut ve Sorgu Handler'ları
-   Repository metodları
-   UsersController (Login)
-   CategoryController

### 🧾 Test Komutu:
```bash
dotnet test
```
### 📊 Coverage Almak:
```bash
dotnet test --collect:"XPlat Code Coverage"
```
### 🧷 Coverage Raporunu Görüntülemek:
```bash
reportgenerator -reports:./**/coverage.cobertura.xml -targetdir:coveragereport
```
## 🧱 Proje Yapısı
```css
📁 Book
├── 📁 Book.API                      # Ana Web API projesi
│   ├── Application/                # CQRS komut ve handler'ları
│   ├── Controllers/                # API Controller'lar
│   ├── Data/                       # AppDbContext ve veritabanı işlemleri
│   ├── Domain/                     # Entity modelleri ve domain kuralları
│   ├── Dto/                        # Veri transfer nesneleri
│   ├── Infrastructure/            # Cross-cutting concern'ler (örnek: servisler)
│   ├── Logging/                   # ConsoleLogger gibi özel loglama sınıfları
│   ├── Mappings/                  # AutoMapper profilleri
│   ├── Middleware/                # Hata yakalama gibi özel middleware'ler
│   ├── Migrations/                # EF Core migration dosyaları
│   ├── Models/                    # DTO dışında kullanılan yardımcı modeller
│   ├── Services/                  # Business logic servisleri
│   ├── appsettings.json           # Ortak ayarlar (JWT, connection string vs.)
│   ├── Program.cs                 # Service registration + middleware pipeline
│   └── Book.API.http              # HTTP test istekleri
│
├── 📁 Book.API.Tests               # Birim test projeleri
│   ├── Categories/                # Kategori testleri
│   ├── Controllers/               # Controller testleri (özellikle Login)
│   ├── Data/                      # In-memory context testleri
│   ├── Helpers/                   # InMemoryDbHelper, MapperHelper vb.
│   ├── Mocks/                     # IConfiguration, ILogger mockları
│   ├── Products/                  # Ürün testleri
│   ├── Repositories/             # Repository katmanı testleri
│   ├── TestResults/              # Test çıktılarının tutulduğu dizin
│   ├── appsettings.json          # Test ortamı için JWT secret ve benzeri
│   └── Usings.cs                 # Global using tanımları (C# 10+ özelliği)
```