using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc;

namespace Dojodachi.Controllers

{

    public class DojoController : Controller
    {
        //  public string message ="";
         public Random chance = new Random();
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        
        {
            if(HttpContext.Session.GetInt32("meals") == null){
                    HttpContext.Session.SetString("alive","alive");
                    HttpContext.Session.SetString("status", "He is Fine!");
                    HttpContext.Session.SetInt32("fullness", 20); 
                    HttpContext.Session.SetInt32("happiness", 20);
                    HttpContext.Session.SetInt32("meals", 3);
                    HttpContext.Session.SetInt32("energy", 50);
                    HttpContext.Session.SetString("message", "");

                }

            int? fullness = HttpContext.Session.GetInt32("fullness");
            int? happiness = HttpContext.Session.GetInt32("happiness");
            int? energy = HttpContext.Session.GetInt32("energy");
            int? meals = HttpContext.Session.GetInt32("meals");
            string message= HttpContext.Session.GetString("message");
            string status = HttpContext.Session.GetString("status");
            string alive = HttpContext.Session.GetString("alive");

            ViewBag.fullness = (int)fullness;
            ViewBag.happiness = (int)happiness;
            ViewBag.energy = (int)energy;
            ViewBag.meals = (int)meals;
            ViewBag.message= message;
            ViewBag.status= status;
            ViewBag.alive = alive;

            if((int)happiness == 100 && (int)fullness == 100){
                HttpContext.Session.SetString("alive","Win");
                ViewBag.status = $"You Won! -- Restart?";
            }
            else if((int)happiness <= 0 || (int)fullness <= 0 || (int)energy <= 0){
                HttpContext.Session.SetString("alive","dead");
                ViewBag.alive = "dead";
                ViewBag.status = $"Your Pet has died. -- Restart?";
            }

            else if((int)happiness <= 15 || (int)fullness <= 15 || (int)energy <= 15){
                ViewBag.status = $"Your Pet is almost dead!";
            }
            
            else{
                ViewBag.status = $"Your Pet is fine!";
            }
            
            return View("Index");
        }   

        [HttpPost]
        [Route("Feed")]
        public IActionResult Feed()
        {
            int? meals = HttpContext.Session.GetInt32("meals");
            int? fullness = HttpContext.Session.GetInt32("fullness") + chance.Next(5,11);
            int? happiness = HttpContext.Session.GetInt32("happiness");
            // int? new_fullness= fullness + chance.Next(5,11);
            
            if(meals > 0){
                meals -= 1;
                HttpContext.Session.SetInt32("meals", (int)meals);
                HttpContext.Session.SetInt32("fullness", (int)fullness);
                HttpContext.Session.SetString("message", $"You fed your Pet. His fullness is now  at {fullness}!");
            }
            if (fullness > 100){
                happiness -= 10;
                HttpContext.Session.SetString("status", "He's been fed enough! Feed him more and his happiness will drop.");
                HttpContext.Session.SetInt32("happiness", (int)happiness);

            }
            if(meals == 0){
                meals = 0;
                HttpContext.Session.SetString("message", $"You have no more meals!");
            }
             return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Play")]
        public IActionResult Play(){
            int? happiness= HttpContext.Session.GetInt32("happiness");
            int? energy= HttpContext.Session.GetInt32("energy");
            happiness += chance.Next(5,10);
            energy -=  10;
            HttpContext.Session.SetInt32("energy", (int)energy);
            HttpContext.Session.SetInt32("happiness", (int)happiness);
            HttpContext.Session.SetString("message", $"You played with your Pet. His happiness increased to {happiness}, but decreased energy to {energy}!");
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("Work")]
            public IActionResult Work(){
            int? energy= HttpContext.Session.GetInt32("energy");
            int? meals= HttpContext.Session.GetInt32("meals");
            int added_meals = chance.Next(1,4);
            Console.WriteLine("added_meals: " + added_meals);
            meals = meals + added_meals;
            energy -=  5;
            HttpContext.Session.SetInt32("added_meals", (int)added_meals);
            HttpContext.Session.SetInt32("meals", (int)meals);
            HttpContext.Session.SetInt32("energy", (int)energy);
            HttpContext.Session.SetString("message", $"Your pet worked. His energy decreased to {energy} but he aquired {added_meals} meals!");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Sleep")]
        public IActionResult Sleep(){
        
            int? energy= HttpContext.Session.GetInt32("energy");
            int? fullness= HttpContext.Session.GetInt32("fullness");
            int? happiness= HttpContext.Session.GetInt32("happiness");
            
            happiness -= 5;
            fullness -= 5;
            energy +=  15;

            HttpContext.Session.SetInt32("energy", (int)energy);
            HttpContext.Session.SetInt32("fullness", (int)fullness);
            HttpContext.Session.SetInt32("happiness", (int)happiness);
            HttpContext.Session.SetString("message","Your Dojodachi slept");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Clear")]
        public IActionResult Clear()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
    }
}