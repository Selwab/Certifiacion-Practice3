using Microsoft.AspNetCore.Mvc;
using UPB.CoreLogic.Managers;
using UPB.CoreLogic.Models;

namespace UPB.PracticeThree.Controllers;

[ApiController] //Attributes
[Route("patient")]

public class PatientController : ControllerBase
{
  //Add constructor and Get() and set manager get too
    private readonly PatientManager _patientManager;

    public PatientController(PatientManager patientManager)
    {
        _patientManager = patientManager;
    }

    [HttpGet]
    public List<Patient> Get()
    {
        return _patientManager.GetAll();
    }

    [HttpPost] 
    public Patient Post([FromBody] Patient patientToCreate)
    {
        return _patientManager.Create(patientToCreate.Name, patientToCreate.LastName, patientToCreate.CI);
    }

    [HttpGet]
    [Route("{ci}")]
    public Patient GetByCi([FromRoute] int ci)
    {
        return _patientManager.GetById(ci);
    }

    [HttpPut]
    [Route("{ci}")]
    public Patient Put([FromRoute] int ci, [FromBody]Patient patientToUpdate)
    {
        return _patientManager.Update(ci, patientToUpdate.Name, patientToUpdate.LastName);
    }

    [HttpDelete]
    [Route("{ci}")]
    public Patient Delete([FromRoute] int ci)
    {
        return _patientManager.Delete(ci);
    }
}
