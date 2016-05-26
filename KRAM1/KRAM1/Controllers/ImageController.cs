﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KRAM1.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web.Routing;
using System.Drawing;

namespace KRAM1.Controllers
    {
    public class ImageController : Controller
        {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Image
        public ActionResult Index()
            {
            var model = context.Pictures.ToList();
            return View(model);
            }

        // GET: Image/Details/5
        public ActionResult Details(int id)
            {

            var list = context.Pictures.ToList();


            return View(list);
            }

        public ActionResult Submit()
            {
            ApplicationDbContext context = new ApplicationDbContext();
            var url = Request["imgurl"];
            var tags = Request["tags"];
            var userId = User.Identity.GetUserId();
            var user = context.Users.Where(u => u.Id==userId);

            if(url==null&&tags==null)
                {

                }
            else
                {
                //   ApplicationUser currentUser = new ApplicationUser() { Id = userId };
                Hashtag hastags = new Hashtag() { Name=tags };
                Picture newImage = new Picture()
                    {
                    PicUrl=url,
                    TimeStamp=DateTime.Now,
                    UserId=userId,
                    Hashtag=hastags
                    };

                context.Pictures.Add(newImage);
                context.SaveChanges();
                }
            return View();
            }
        public ActionResult FullImage(int fileName)
            {

            var x = context.Pictures.Find(fileName);
         
            ViewBag.Hashtag=x.Hashtag;
            ViewBag.x=x.PicUrl;
            return View();
            }
        [HttpPost]

        public ActionResult Upload(Picture newPicture, HttpPostedFileBase file)
            {
            int fileName = 0;
            ApplicationDbContext context = new ApplicationDbContext();

                if(file==null)
                    {
                    ModelState.AddModelError("File", "Please Upload Your imgfile");
                    }
                    else
                        {    
                
                        file.ValidateImageFile();
                        string extension = Path.GetExtension(file.FileName);
                        var tags = Request["tags"];
               
                        var userId = User.Identity.GetUserId();
                     

                        var fileNames = Path.GetFileName(file.FileName);
                        var guid = Guid.NewGuid().ToString(); //Randomizer filnamnet
                        var path = Path.Combine(Server.MapPath("~/uploads"), guid+fileNames); //Får fram fullständiga mappen man sparar i. Vi får ändra till server mappen senare.
                       
                        string fl = path.Substring(path.LastIndexOf("\\"));
                        string[] split = fl.Split('\\');

                        string newpath = split[1];
                      
                        string imagepath = "~/uploads/"+newpath;
                        Hashtag hastags = new Hashtag() { Name=tags };
                        //Binder till bildtabellen i databasen
                        newPicture.PicUrl=imagepath; //Får nog kanske göra om imagepath / path senare när vi laddar upp den till en server.
                        newPicture.TimeStamp=DateTime.Now;
                        newPicture.Hashtag=hastags;
                        newPicture.UserId=userId;
   

                        file.SaveAs(path); //Sparar till en mapp ~/uploads/
                        context.Pictures.Add(newPicture);
                        context.SaveChanges();
                         ModelState.Clear();
                        ViewBag.Message = "Image uploaded successfully";
                        fileName=newPicture.Id;
                      return RedirectToAction("FullImage", "Image", new { fileName = fileName });
                        }
                    

                //TempData["Success"]="Upload successful";
                    return RedirectToAction("Index");
                }


                    }
              

            }
        
    
