using Microsoft.AspNetCore.Mvc;
using UPB.CoreLogic.Managers;
using UPB.CoreLogic.Models;

namespace UPB.PracticeThree.Controllers;

[ApiController] //Attributes
[Route("[controller]")]

public class PatientController : ControllerBase
{
  //Add constructor and Get() and set manager get too
    private readonly PatientManager _patientManager;

    public PatientController(PatientManager patientManager)
    {
        _patientManager = patientManager;
    }
}
