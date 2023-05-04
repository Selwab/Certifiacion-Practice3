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
}
