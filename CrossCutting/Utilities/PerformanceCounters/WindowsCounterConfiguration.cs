using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    public class WindowsCounterConfiguration :
        ICounterConfiguration
    {
        //key: CategoryName
        //value: CounterCreationInfo
        readonly List<CategoryConfiguration> _categoryConfigurations =
            new List<CategoryConfiguration>();

        public void Register<TCounterCategory>() where TCounterCategory : CounterCategory
        {
            Register(typeof(TCounterCategory));
        }
        void Register(Type counterCategoryType)
        {
            var category = new CategoryConfiguration
                {
                    Name = counterCategoryType.Name
                };
            var props = counterCategoryType.GetProperties();
            foreach (var propertyInfo in props)
            {
                var counter = new PerformanceCounterConfiguration(propertyInfo.Name, "no-counter-help-yet",
                                                               PerformanceCounterType.NumberOfItems32);
                category.Counters.Add(counter);
            }
            _categoryConfigurations.Add(category);
            
        }

        public CounterRepository BuildRepository()
        {
            var repo = new CounterRepository();
            _categoryConfigurations.ForEach(repo.RegisterCategory);
            return repo;
        }
    }
}