namespace DevPodcasts.DataLayer.Models
{
    public class CheckBoxListItem : ModelBase<int>
    {
        public string Display { get; set; }
        public bool IsChecked { get; set; }
    }
}
