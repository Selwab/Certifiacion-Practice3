using UPB.CoreLogic.Models;

namespace UPB.CoreLogic.Managers;

public class PatientManager
{
    private List<Patient> _patients;

    public PatientManager()
    {
        _patients = new List<Patient>();
    }

    public List<Patient> GetAll()
    {
        return _patients;
    }

    public Patient Create(string name, string lastName, int ci)
    {
        string bloodType = GetBloodType();

        Patient createdPatient = new Patient()
        {
            Name = name,
            LastName = lastName,
            CI = ci,
            BloodType = bloodType
        };
        _patients.Add(createdPatient);
        return createdPatient;
    }

    public string GetBloodType()
    {
        string[] bloodGroups = new string[] {"A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"};
        Random random = new Random();
        int index = random.Next(0,bloodGroups.Length);
        return bloodGroups[index];
    }

     public Patient GetById(int ci)
    {
        if(ci < 0)
        {
            throw new Exception("CI invalido");
        }

        Patient patientFound;
        patientFound = _patients.Find(patient => patient.CI == ci);

        if(patientFound == null)
        {
            throw new Exception("No se encontró ningún paciente con el CI: " + ci);
        }
        return patientFound;
    }
}
