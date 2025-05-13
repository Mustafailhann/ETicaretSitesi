# ETicaretSitesi

Bu proje, C# ile geliştirilen katmanlı mimariye sahip bir ASP.NET MVC E-Ticaret uygulamasıdır. Uygulama, ürün listeleme, sepete ekleme, sipariş verme gibi temel e-ticaret işlevlerini destekler.

## 🧰 Kullanılan Teknolojiler

- ASP.NET MVC 5
- Entity Framework
- Katmanlı Mimari (Business, Data Access, Entities, UI)
- SQL Server (veritabanı bağlantısı gereklidir)
- Razor View Engine
- LINQ
- C#

## 📁 Katman Yapısı

- **ETicaretMvcWebUI** – Kullanıcı arayüzü (MVC)
- **ETicaretBusiness** – İş mantığı katmanı
- **ETicaretDal** – Veri erişim katmanı (Repository Pattern)
- **ETicaretEntities** – Entity sınıfları

## 🚀 Başlatma Adımları

1. Bu repoyu klonlayın veya zip olarak indirin:
   ```bash
   git clone https://github.com/Mustafailhann/ETicaretSitesi.git
   Visual Studio ile ETicaretSitesi.sln dosyasını açın.

Gerekli NuGet paketlerini yükleyin.

Veritabanı bağlantı ayarlarını web.config dosyasında yapılandırın.

Veritabanını oluşturun ve gerekli tabloları ekleyin.

Uygulamayı başlatın.

## 🛒 Özellikler

- Kategoriye göre ürün listeleme
- Ürün detay sayfası
- Sepete ürün ekleme ve çıkarma
- Sipariş oluşturma
- Repository Pattern kullanımı ile temiz veri erişimi
- Katmanlı mimariyle kod okunabilirliği ve sürdürülebilirliği

## 🖼 Ekran Görüntüleri

> Ekran görüntülerini `screenshots/` klasörüne ekleyebilir ve burada gösterebilirsin:
>
> ![Ana Sayfa](screenshots/homepage.png)
> ![Ürün Detay](screenshots/product-detail.png)

## 🛠 Geliştirme Önerileri

- ASP.NET Identity ile kullanıcı yönetimi ve giriş sistemi entegre edilebilir
- Admin paneli geliştirilebilir (ürün ekleme, sipariş kontrolü vs.)
- Sepet işlemleri client-side tarafına taşınabilir (örneğin JavaScript ile)
- Test otomasyonları eklenebilir (xUnit, NUnit)
- RESTful API servisi yazılarak mobil destek sağlanabilir

## 💡 Katkı Sağlamak

Katkı sağlamak isterseniz:

1. Fork'layın
2. Yeni bir branch oluşturun (`feature/yenilik`)
3. Değişiklikleri yapın ve commit'leyin
4. Pull Request gönderin

## 📄 Lisans

Bu proje açık kaynaklıdır ve lisanssız olarak paylaşılmıştır. Eğitim ve geliştirme amaçlı kullanılabilir.
