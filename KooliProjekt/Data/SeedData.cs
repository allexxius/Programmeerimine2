using KooliProjekt.Data;
using KooliProjekt.Data.Migrations;
using KooliProjekt.Models;
using System;
using System.Linq;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext dbContext)
        {
            // Seed Doctors if the table is empty
            if (!dbContext.Doctors.Any())
            {
                dbContext.Doctors.AddRange(
                    new Doctor { Name = "Dr. John Doe", Specialization = "Cardiology" },
                    new Doctor { Name = "Dr. Jane Smith", Specialization = "Neurology" },
                    new Doctor { Name = "Dr. Alice Jones", Specialization = "Pediatrics" },
                    new Doctor { Name = "Dr. Bob Martin", Specialization = "Orthopedics" },
                    new Doctor { Name = "Dr. Carol White", Specialization = "Dermatology" },
                    new Doctor { Name = "Dr. David Brown", Specialization = "Psychiatry" },
                    new Doctor { Name = "Dr. Emily Green", Specialization = "Radiology" },
                    new Doctor { Name = "Dr. Frank Harris", Specialization = "Oncology" },
                    new Doctor { Name = "Dr. Grace King", Specialization = "Gastroenterology" },
                    new Doctor { Name = "Dr. Henry Lee", Specialization = "Endocrinology" }
                );
            }

            // Seed Documents if the table is empty
            if (!dbContext.Documents.Any())
            {
                dbContext.Documents.AddRange(
                    new Document { Type = "PDF", File = "Medical_Report_2023.pdf" },
                    new Document { Type = "Word Document", File = "Patient_History.docx" },
                    new Document { Type = "Image", File = "Xray_Image.png" },
                    new Document { Type = "Excel Sheet", File = "Lab_Results.xlsx" },
                    new Document { Type = "Presentation", File = "Health_Tips.pptx" },
                    new Document { Type = "PDF", File = "Surgery_Notes.pdf" },
                    new Document { Type = "Word Document", File = "Prescription.docx" },
                    new Document { Type = "Image", File = "MRI_Scan.png" },
                    new Document { Type = "PDF", File = "Discharge_Summary.pdf" },
                    new Document { Type = "Text File", File = "Followup_Notes.txt" }
                );
            }

            // Save all changes to the database
            dbContext.SaveChanges();
        }
    }
}