namespace KooliProjekt.WinFormsApp.Api
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; } // Add this required field
        public string Title { get; set; }
    }
}