using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models
{
    public class InventoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public Gender Gender { get; set; }
        public decimal PricePerHour { get; set; }
        public int Amount { get; set; }

        private InventoryModel(Guid id, string name, int size, Gender gender, decimal pricePerHour, int amount) 
        {
            Id = id;
            Name = name;
            Size = size;
            Gender = gender;
            PricePerHour = pricePerHour;
            Amount = amount;
        }

        public static(InventoryModel InventoryModel, string error) Create(Guid id, string name, int size, Gender gender, decimal pricePerHour, int amount)
        {
            var inventory = new InventoryModel(id, name, size, gender, pricePerHour, amount);

            string errorString = string.Empty;

            string genderString = gender.ToString();

            if (string.IsNullOrEmpty(name))
            {
                errorString = "Не введено название инвентаря";
            }
            else if (size > 50 || size < 20 )  
            {
                errorString = "Введены некорректные размеры коньков";
            }
            else if (string.IsNullOrEmpty(genderString))
            {
                errorString = "Введены некорректные данные";
            }
            else if (pricePerHour < 0) 
            {
                errorString = "Стоимость не может быть отрицательным";
            }
            else if (amount < 0)
            {
                errorString = "Количество не может быть отрицательным";
            }

            if(!string.IsNullOrEmpty(errorString))
            {
                return (null, errorString);
            }

            return (inventory, errorString);
        }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
