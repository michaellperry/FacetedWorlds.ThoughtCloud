using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using FacetedWorlds.ThoughtCloud.ViewModel;
using UpdateControls.XAML.Wrapper;

namespace FacetedWorlds.ThoughtCloud.ValueConverters
{
    public class TabConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<object> sourceCollection = value as ObservableCollection<object>;

            if (sourceCollection != null)
            {
                ObservableCollection<TabItem> targetCollection = new ObservableCollection<TabItem>();
                PopulateTargetCollection(sourceCollection, targetCollection);

                var notifyCollectionChanged = sourceCollection as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                    notifyCollectionChanged.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
                    {
                        if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            // Add the corresponding targets.
                            int index = e.NewStartingIndex;
                            foreach (object item in e.NewItems)
                            {
                                targetCollection.Insert(index, CreateTabItem(item));
                                ++index;
                            }
                        }
                        else if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            // Delete the corresponding targets.
                            for (int i = 0; i < e.OldItems.Count; i++)
                            {
                                targetCollection.RemoveAt(e.OldStartingIndex);
                            }
                        }
                        else if (e.Action == NotifyCollectionChangedAction.Replace)
                        {
                            // Replace the corresponding targets.
                            for (int i = 0; i < e.OldItems.Count; i++)
                            {
                                targetCollection[i + e.OldStartingIndex] = CreateTabItem(e.NewItems[i + e.NewStartingIndex]);
                            }
                        }
                        else
                        {
                            // Just give up and start over.
                            targetCollection.Clear();
                            PopulateTargetCollection(sourceCollection, targetCollection);
                        }
                    };

                return targetCollection;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private void PopulateTargetCollection(ObservableCollection<object> sourceCollection, ObservableCollection<TabItem> targetCollection)
        {
            foreach (object item in sourceCollection)
            {
                targetCollection.Add(CreateTabItem(item));
            }
        }

        private TabItem CreateTabItem(object item)
        {
            IObjectInstance wrapper = item as IObjectInstance;
            CloudTabViewModel tabViewModel = wrapper.WrappedObject as CloudTabViewModel;
            return new TabItem { Header = tabViewModel.Header, Content = new TextBlock { Text = tabViewModel.Header } };
        }
    }
}
