using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KitapListesiCMS.Controllers
{
    public class HomeController : Controller
    {
        public KitaplarEntities db = new KitaplarEntities();
        public string publicKullaniciAdi = "User";
        public string publicSifre = "Pass";

        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(FormCollection fc)
        {
             string kullaniciAdi = fc["kullaniciAdi"];
             string sifre = fc["sifre"];

            if(kullaniciAdi==publicKullaniciAdi && sifre==publicSifre){
               Session["Yetki"]=kullaniciAdi;
                return Redirect("/Home/Index");
            }
            return View();
        }
        //kullanıcı girince kitap listelenmesi
        public ActionResult Index()
        {
            if (Session["Yetki"]!=null)
            {
                List<KitapListesi> kitaplar = db.KitapListesi.ToList();
                return View(kitaplar);
            }
            else
            {
                return Redirect("/Home/Giris");
            }
            
            
        }
        //Veri Ekleme
        public ActionResult Ekle()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Ekle(FormCollection fc)
        {
            if (Session["Yetki"] != null)
            {
                string kitapAdi = fc["kitapAdi"];
                string yazar = fc["yazar"];
                string fiyat = fc["fiyat"];
                string tarih = fc["tarih"];

                KitapListesi yeniKitap = new KitapListesi
                {
                    KitapAdi = kitapAdi,
                    Yazar = yazar,
                    Fiyat = Convert.ToDecimal(fiyat),
                    Tarih = Convert.ToDateTime(tarih)
                };
                db.KitapListesi.Add(yeniKitap);
                db.SaveChanges();
            }
            return Redirect("/Home/Index");

        }
        public ActionResult Guncelle()
        {
            if (Session["Yetki"] != null)
            {
                int id = int.Parse(Request.QueryString["id"]);
                KitapListesi kitap = db.KitapListesi.Where(x => x.Id == id).FirstOrDefault();
                return View(kitap);
            }
            else
            {
                return Redirect("/Home/Giris");
            }
        }
        //Veri Güncelleme
        [HttpPost]
        public ActionResult Guncelle(FormCollection fc)
        {
            if (Session["Yetki"] != null)
            {
                int id = int.Parse(fc["id"]);
                string kitapAdi = fc["kitapAdi"];
                string yazar = fc["yazar"];
                string fiyat = fc["fiyat"];
                string tarih = fc["tarih"];

                KitapListesi guncellenecekKitap = db.KitapListesi.Where(x => x.Id == id).FirstOrDefault();
                guncellenecekKitap.Id = id;
                guncellenecekKitap.KitapAdi = kitapAdi;
                guncellenecekKitap.Yazar = yazar;
                guncellenecekKitap.Fiyat = Convert.ToDecimal(fiyat);
                guncellenecekKitap.Tarih = Convert.ToDateTime(tarih);

                db.SaveChanges();
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Giris");

            }
            

        }
        public ActionResult Sil()
        {
            if (Session["Yetki"] != null)
            {
                int id = int.Parse(Request.QueryString["id"]);
                KitapListesi silinecekKitap=db.KitapListesi.Where(x => x.Id == id).FirstOrDefault();
                db.KitapListesi.Remove(silinecekKitap);
                db.SaveChanges();
            }
            return Redirect("/Home/Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}