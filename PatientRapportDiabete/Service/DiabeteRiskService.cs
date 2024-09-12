using PatientRapportDiabete.Models;

namespace PatientRapportDiabete.Service
{
    public class DiabeteRiskService
    {
        private readonly NoteService noteService;
        private readonly PatientService patientService;
        private string[] terms = [
            "Hémoglobine A1C",
            "Microalbumine",
            "Taille",
            "Poids",
            "Fumeur",
            "Fumeuse",
            "Anormal",
            "Cholestérol",
            "Vertiges",
            "Rechute",
            "Réaction",
            "Anticorps"
        ];
        public DiabeteRiskService(NoteService noteService, PatientService patientService)
        {
            this.noteService = noteService;
            this.patientService = patientService;
        }
        public async Task<RiskEnum?> GetRisk(int id, string token)
        {
            var patient = await patientService.Get(id, token);
            
            if (patient == null)
            {
                return null;
            }

            var age = CalculateAge(patient.DateOfBirth);
            var notes = await noteService.Get(id, token);
            
            if (notes is null)
            {
                return null;
            }

            int nbterms = 0;

            foreach (var note in notes)
            {
                foreach (var term in terms)
                {
                    if (note.Content.Contains(term, StringComparison.OrdinalIgnoreCase))
                    {
                        nbterms++;
                    }
                }
            }

            if (age > 30)
            {
                if (nbterms >= 2 && nbterms <= 5)
                {
                    return RiskEnum.Borderline;
                }
                if (nbterms >= 6 && nbterms <= 7)
                {
                    return RiskEnum.InDanger;
                }
                if (nbterms >= 8)
                {
                    return RiskEnum.EarlyOnset;
                }
            }

            if (string.Equals(patient.Gender, "M"))
            {
                if (nbterms >= 5)
                {
                    return RiskEnum.EarlyOnset;
                }
                if (nbterms >= 3)
                {
                    return RiskEnum.InDanger;
                }
            }

            if (string.Equals(patient.Gender, "F"))
            {
                if (nbterms >= 8)
                {
                    return RiskEnum.EarlyOnset;
                }
                if (nbterms >= 4)
                {
                    return RiskEnum.InDanger;
                }
            }
            return RiskEnum.None;
        }
        private static int CalculateAge(DateTime dateOfBirth)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;

            if (now < dateOfBirth.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
