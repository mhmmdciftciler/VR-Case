Proje için kısa notlar.
- Fusion 2.0 sürümü ve VR addonları kullanıldı. Basitçe entegre edildiğinden tercihim oldu.
- HDRP kullanıldı. Tek sahnede eldeki assetlere uygun bir sahne hazırlandı.
- Outline için daha önce yazılmış HDRP de çok güzel bir yöntem olan Custom Pass yöntemi kullanıldı. Objelerin Layerı değişiyor. Bu yöntemi sevmiyorum fakat bu proje özelinde üzerine gitmek istemedim. En azından renderer seviyesine indirgenirse Layer değişimi için öngörülen sorunlar çözülür.
- Yolladığınız assette optimize edilmesi gereken bazı işler vardı. Elimden geldiğince yaptım. Örneğin meshler çok büyük alanları kaplıyor ve gereksiz renderlanıyor. Bunun için özellikle yüksek trisli objeler için değişiklik yaptım. İsimlendirmesi çok kötü o yüzden o konuda çok uğraşmadım.
- Photon addonu Shared Mode ile çalışıyor. Host-Clienti denemedim.
- Photon sski sürümlerde hatırladığım kadarıyla Networked ile işaretlenmiş parametrelere OnChanged eventi eklenebiliyordu. Sanırım artık o kaldırılmış. O yüzden pallet üzerinde bulunan parametre için eski durumu kontrol eden bir kod yazmak durumunda kaldım. Araştırınca buna benzer bir yapı çıktı(ChangeDetector) ama çok fark göremedim o yüzden kendi bildiğim gibi yaptım.

Genel hatlarıyla çalışmanın özeti.

- Device simulator kullanıldı.
- XR Interaction kullanıldı.
- VR locomotion ve Box taşınması için Fusion VR addonu kullanıldı.
- Board için kuralların olduğu Rule Service yazıldı.
- Board için UI kodu yazıldı.
- Board paletlerin durumunu dinler.
- Paletlerin üzerine bırakılan box varsa yeni box alamaz. Üzeri boş ise box bırakıldığın o box tipini bildirir.
- Bir box tutulduğunda boş paletler outline açar.