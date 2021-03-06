﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GuessTheAnimal.Models;

namespace GuessTheAnimal.BusinessLayer
{
    public class BLAnimalDetail
    {
        private AnimalEntities db = new AnimalEntities();

        /// <summary>
        /// Retrieve one fact(Animal Detail) for each animal.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="currentFact"></param>
        /// <returns></returns>
        public AnimalDetail GetFactForNextAnimal(List<AnimalDetail> items, AnimalDetail currentFact)
        {
            if (currentFact != null)
            {
                int nextAnimalId = currentFact.AnimalId + 1;
                var animalDetails = from element in db.AnimalDetails.Include(a => a.Animal)
                                    group element by element.AnimalId
                                    into groups
                                    select groups.OrderBy(p => p.FactId).FirstOrDefault();
                return animalDetails.Include(a => a.Animal).ToList().Where(ad => ad.AnimalId == nextAnimalId).OrderBy(a => a.FactId).FirstOrDefault();
                //from element in db.AnimalDetails.Include(a => a.Animal)
                //                select groups.OrderBy(p => p.FactId).FirstOrDefault();
            }
            return null;
            
        }

        /// <summary>
        /// if one fact is matched then retrieve the next fact for the same Animal.
        /// </summary>
        /// <param name="currentFact"></param>
        /// <returns></returns>
        public AnimalDetail GetNextFactForAnimal(AnimalDetail currentFact)
        {
            if (currentFact != null)
            {
                int nextAnimalId = currentFact.AnimalId + 1;

                var items = db.AnimalDetails.Include(a => a.Animal).Where(ad => ad.AnimalId == currentFact.AnimalId).OrderBy(a => a.FactId).ToList();
                int index = items.FindIndex(it => it.FactId == currentFact.FactId);

                if (items.Count > index + 1)
                {
                    return items.ElementAt(index + 1);
                }
                else {
                   
                }
                
            }
            return null;

        }
    }
}