using UPB.CoreLogic.Models;

namespace UPB.CoreLogic.Managers;

public class PatientManager
{
    private List<Patient> _patients;
    private string _filePath;

    public PatientManager(string filePath)
    {
        _patients = new List<Patient>();
        _filePath = filePath;
    }

    public List<Patient> GetAll()
    {
        List<Patient> patients = new List<Patient>();

        using (StreamReader reader = new StreamReader(_filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] patientData = line.Split(',');
                string name = patientData[0];
                string lastName = patientData[1];
                int ci = Int32.Parse(patientData[2]);
                string bloodType = patientData[3];

                Patient Patient = new Patient
                {
                    Name = name,
                    LastName = lastName,
                    CI = ci,
                    BloodType = bloodType
                };
                patients.Add(Patient);
            }
        }

        return patients;
    }

    public Patient Create(string name, string lastName, int ci)
    {
        if(ci < 0)
        {
            throw new Exception("CI invalido");
        }

        using (StreamReader reader = new StreamReader(_filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] patientData = line.Split(',');
                int patientCI = Int32.Parse(patientData[2]);

                if(ci == patientCI)
                {
                    throw new Exception("El CI " + ci + " ya existe");
                }
            }
        }

        string bloodType = GetBloodType();
        Patient createdPatient = new Patient()
        {
            Name = name,
            LastName = lastName,
            CI = ci,
            BloodType = bloodType
        };

        using (StreamWriter writer = new StreamWriter(_filePath, true))
        {
            string patientData = string.Join(",", name, lastName, ci, bloodType);
            writer.WriteLine(patientData);
        }

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
        Patient patientFounded = null; 

        if(ci < 0)
        {
            throw new Exception("CI invalido");
        }

        using (StreamReader reader = new StreamReader(_filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] patientData = line.Split(',');
                string name = patientData[0];
                string lastName = patientData[1];
                int patientCI = Int32.Parse(patientData[2]);
                string bloodType = patientData[3];

                if(ci == patientCI)
                {
                    patientFounded = new Patient
                    {
                        Name = name,
                        LastName = lastName,
                        CI = ci,
                        BloodType = bloodType
                    };
                    break;
                }
            }
        }

        if(patientFounded == null)
        {
            throw new Exception("No se encontró ningún paciente con el CI: " + ci);
        }
        return patientFounded;
    }
    public Patient Update(int ci, string name, string lastName)
    {
        Patient patientFounded = null; 

        if(ci < 0)
        {
            throw new Exception("CI invalido");
        }

        string[] fileContent = File.ReadAllLines(_filePath);

        for (int i = 0; i < fileContent.Length; i++)
        {
            string[] patientData = fileContent[i].Split(',');
            int patientCI = Int32.Parse(patientData[2]);
            string bloodType = patientData[3];

            //Console.WriteLine("Patient CI: " + patientCI);
            
            if(ci == patientCI)
            {
                fileContent[i] = $"{name},{lastName},{ci},{bloodType}";

                patientFounded = new Patient
                {
                    Name = name,
                    LastName = lastName,
                    CI = ci,
                    BloodType = bloodType
                };
                break;
            }
        }
        
        if(patientFounded == null)
        {
            throw new Exception("No se encontró ningún paciente con el CI: " + ci);
        }

        File.WriteAllLines(_filePath, fileContent);
        return patientFounded;
    }

    public Patient Delete(int ci)
    {
        Patient patientToDelete = null; 

        if (ci < 0)
        {
            throw new Exception("CI invalido");
        }

        List<string> fileContent = File.ReadAllLines(_filePath).ToList();

        for (int i = 0; i < fileContent.Count; i++)
        {
            string[] patientData = fileContent[i].Split(',');
            string name = patientData[0];
            string lastName = patientData[1];
            int patientCI = Int32.Parse(patientData[2]);
            string bloodType = patientData[3];

            if(ci == patientCI)
            {
                patientToDelete = new Patient
                {
                    Name = name,
                    LastName = lastName,
                    CI = ci,
                    BloodType = bloodType
                };

                fileContent.RemoveAt(i);

                File.WriteAllLines(_filePath, fileContent);

                break;
            }
        }

        if(patientToDelete == null)
        {
            throw new Exception("No se encontró ningún paciente con el CI: " + ci);
        }

        return patientToDelete;
    }
}
