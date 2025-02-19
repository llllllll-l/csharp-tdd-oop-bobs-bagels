using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exercise.main
{
    public class Basket1
    {
        private IInventory _inventory;
        private int _capacity;
        private List<IProduct> _products = new List<IProduct>();

        public Basket1(IInventory inventory, int capacity) {
            _inventory = inventory;
            _capacity = capacity;
        }

        /* 1. As a member of the public,
        So I can order a bagel before work,
        I'd like to add a specific type of bagel to my basket.
        */
        public bool AddToBasketIfExists(string sku, List<string> extraSelectedFillings, out IProduct product) {
            if (_inventory.IsItemInStock(sku)) {
                product = _inventory.GetProduct(sku);

                if (!isFull()) {
                    if (product is IFillable fillable) {
                        List<Tuple<string, decimal>> additionalFillings = new List<Tuple<string, decimal>>();

                        if (extraSelectedFillings != null) {
                            foreach (string filling in extraSelectedFillings) {
                                if (_inventory.IsItemInStock(filling)) {
                                    Product fillingItem = _inventory.GetFilling(filling);
                                    if (fillingItem != null && fillingItem.Category.StartsWith("Filling")) {
                                        additionalFillings.Add(new Tuple<string, decimal>(fillingItem.Variant, _inventory.GetProductPrice(fillingItem.Sku)));
                                    }
                                    else {
                                        throw new Exception($"Invalid filling selected: {filling}");
                                    }
                                }
                                else {
                                    throw new Exception($"Filling not in stock: {filling}");
                                }
                            }
                        }
                        fillable._fillings.AddRange(additionalFillings);
                    }
                    _products.Add(product);
                    return true;
                }

                throw new Exception("Basket is full");
            }

            throw new Exception($"Product not in stock: {sku}");
        }

        /*2. As a member of the public,
        So I can change my order,
        I'd like to remove a bagel from my basket.
        */
        public bool RemoveFromBasket(string sku) {
            for (int i = 0; i < _products.Count; i++) {
                if (_products[i] is IProduct product) {
                    if (product.Sku.Equals(sku)) {
                        _products.RemoveAt(i);
                        return true;
                    }
                }
            }

            /*5. As a member of the public
            So that I can maintain my sanity
            I'd like to know if I try to remove an item that doesn't exist in my basket.
            */
            throw new Exception("No such product is in the basket");
        }


        /*6. As a customer,
        So I know how much money I need,
        I'd like to know the total cost of items in my basket.
        */
        public decimal TotalCostOfBasket() {
            decimal totalCost = 0;
            foreach (var product in _products) {
                totalCost += product.Price;

                if (product is IFillable fillableProduct) {
                    foreach (var filling in fillableProduct._fillings) {
                        totalCost += filling.Item2;
                    }
                }
            }
            return totalCost;
        }


        /*4. As a Bob's Bagels manager,
        So that I can expand my business,
        I’d like to change the capacity of baskets.
        */
        public void ChangeBasketCapacity(int newCapacity) {
            if (newCapacity > 0) {
                _capacity = newCapacity;
            }
            throw new Exception("Invalid change of basket size");
        }

        private bool isFull()
        {
            return _products.Count >= _capacity ? true : false;
        }

        public int GetCapacity()
        {
            return _capacity;
        }
    }
}