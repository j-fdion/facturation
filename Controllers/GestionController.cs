using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppFactu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AppFactu.Models;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;
using AppFactu.Extensions;
using vueAppFactu.Models;
using vueAppFactu.Utils;

namespace AppFactu.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GestionController : Controller
    {
        private readonly IGestionService _formationService;

        public GestionController(IGestionService formationService)
        {
            _formationService = formationService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("[action]")]
        public IActionResult FeuillePresence(Guid id)
        {

            var session = _formationService.GetSessionForId(id);
            return View(session);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Facture(Guid id)
        {
            var facture = _formationService.GetFactureForId(id);

            DateTimeOffset offset = facture.Date ?? DateTimeOffset.Now;
            DateTime dateFacture = offset.DateTime;
            string idFacture = facture.NumericId.ToString("000");
            facture.NoFacture = dateFacture.ToString("'F'yy'"+ "-" + "'MMdd" + "-" + idFacture);

            return View(facture);
        }

        public async Task<IActionResult> CertificatParticipation(Guid personneId, Guid sessionId)
        {
            var participantSession = _formationService.GetParticipantSessionForPersonneIdAndSessionId(personneId, sessionId);

            var certificat = new CerticatParticipation
            {
                personne = _formationService.GetPersonneForId(personneId),
                session = _formationService.GetSessionForId(sessionId)
            };

            return View(certificat);
        }

        public async Task<IActionResult> CarteParticipation(Guid personneId, Guid sessionId)
        {
            var certificat = new CerticatParticipation
            {
                personne = _formationService.GetPersonneForId(personneId),
                session = _formationService.GetSessionForId(sessionId)
            };

            return View(certificat);
        }

        public async Task<IActionResult> CreerCertificatParticipation()
        {
            return View();
        }

        [HttpGet("[action]")]
        public JsonResult LoadSessionEvents(string start, string end)
        {
            var startDate = DateTimeOffset.Parse(start);
            var endDate = DateTimeOffset.Parse(end);

            var sessions = _formationService.GetSessionsBetweenDates(startDate, endDate);

            var events = new List<Object>();
            foreach (var session in sessions)
            {
                var formateurId = session.Formateur.Id.ToByteArray();
                //var salleId = session.Salle.Id.ToByteArray();
                //var newId = formateurId.XOR(salleId);
                //var id = new Guid(newId);

                var color = session.Formateur.Id.ToHexColor();

                var resourceIds = new List<Guid>();
                resourceIds.Add(session.Formateur.Id);
                double duree = 0.0;
                if (session.UtiliseDureeSession)
                    duree = (double)session.Duree;
                else
                    duree = (double)session.Formation.Duree;
                events.Add(new
                {
                    Id = session.Id,
                    ResourceIds = resourceIds,
                    Title = session.Formation.Titre,
                    Start = session.Date.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    End = session.Date.Value.AddHours(duree).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Color = color,
                    Session = session
                });
            }

            return Json(events);
        }

        [HttpGet("[action]")]
        public IEnumerable<Resource> LoadSessionRessources()
        {
            var formateurs = _formationService.GetFormateurs();
            //var salles = _formationService.GetSalles();
            var resources = new List<Resource>();
            foreach (var formateur in formateurs)
            {
                //foreach (var salle in salles)
                //{
                //var formateurId = formateur.Id.ToByteArray();
                //var salleId = salle.Id.ToByteArray();
                //var newId = formateurId.XOR(salleId);
                //var id = new Guid(newId);
                resources.Add(new Resource
                {
                    Id = formateur.Id,
                    Salle = "",//salle.Nom,
                    Title = formateur.Prenom + " " + formateur.Nom
                });
                //}
            }

            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSessionEvent(Event ev)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }




            return Ok();
        }

        #region Personne
        [HttpPost("[action]")]
        public JsonResult Personnes([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetPersonnes();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetPersonnes().Where(i => i.Prenom.Contains(pagination.Search) || i.Nom.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Personne>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Personne>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SavePersonne([FromBody] Personne item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SavePersonneAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeletePersonne([FromBody] Personne item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeletePersonneAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("[action]")]
        public IEnumerable<Entreprise> SearchEntreprises([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                return _formationService.GetEntreprises().Where(i => i.Nom.Contains(searchTerm));
            }
            else
            {
                return new List<Entreprise>();
            }
        }
        #endregion

        #region Entreprise
        [HttpPost("[action]")]
        public JsonResult Entreprises([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetEntreprises();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetEntreprises().Where(i => i.Nom.Contains(pagination.Search) || i.Nom.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Entreprise>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Entreprise>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveEntreprise([FromBody] Entreprise item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveEntrepriseAsync(item);
            if (result != null)
            {
                result = _formationService.GetEntrepriseForId(item.Id);
                return Ok(result);
            }
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteEntreprise([FromBody] Entreprise item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteEntrepriseAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("[action]")]
        public IEnumerable<ContactEntreprise> SearchContacts([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                var contactEntreprises = _formationService.GetContacts()
                    .Where(i => i.Prenom.Contains(searchTerm) || i.Nom.Contains(searchTerm))
                    .Select(contact => new ContactEntreprise
                    {
                        ContactId = contact.Id,
                        Contact = contact
                    }).ToList();

                return contactEntreprises;
            }
            else
            {
                return new List<ContactEntreprise>();
            }
        }
        #endregion

        #region Formateur
        [HttpPost("[action]")]
        public JsonResult Formateurs([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetFormateurs();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetFormateurs().Where(i => i.Prenom.Contains(pagination.Search) || i.Nom.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Formateur>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Formateur>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveFormateur([FromBody] Formateur item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveFormateurAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteFormateur([FromBody] Formateur item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteFormateurAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("[action]")]
        public IEnumerable<FormateurFormation> SearchFormateurFormations([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                var formateurFormations = _formationService.GetFormations()
                    .Where(i => i.Titre.Contains(searchTerm))
                    .Select(formation => new FormateurFormation
                    {
                        FormationId = formation.Id,
                        Formation = formation
                    }).ToList();

                return formateurFormations;
            }
            else
            {
                return new List<FormateurFormation>();
            }
        }
        #endregion

        #region TypeAccessoire
        [HttpPost("[action]")]
        public JsonResult TypeAccessoires([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetTypeAccessoires();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetTypeAccessoires().Where(i => i.Nom.Contains(pagination.Search) || i.Nom.Contains(pagination.Search));
                int totalRows = results.Count();

                Util.Paginate<TypeAccessoire>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Personne>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveTypeAccessoire([FromBody] TypeAccessoire item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveTypeAccessoireAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteTypeAccessoire([FromBody] TypeAccessoire item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteTypeAccessoireAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }
        #endregion

        #region Accessoire
        [HttpPost("[action]")]
        public JsonResult Accessoires([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetAccessoires();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetAccessoires().Where(i => i.Modele.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Accessoire>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Accessoire>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveAccessoire([FromBody] Accessoire item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveAccessoireAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteAccessoire([FromBody] Accessoire item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteAccessoireAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("[action]")]
        public IEnumerable<TypeAccessoire> SearchTypeAccessoires([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                return _formationService.GetTypeAccessoires().Where(i => i.Nom.Contains(searchTerm));
            }
            else
            {
                return new List<TypeAccessoire>();
            }
        }
        #endregion

        #region Formation
        [HttpPost("[action]")]
        public JsonResult Formations([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetFormations();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetFormations().Where(i => i.Titre.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Formation>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Formation>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveFormation([FromBody] Formation item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveFormationAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteFormation([FromBody] Formation item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteFormationAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }
        #endregion

        #region Contact
        [HttpPost("[action]")]
        public JsonResult Contacts([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetContacts();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetContacts().Where(i => i.Prenom.Contains(pagination.Search) || i.Nom.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Contact>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Contact>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveContact([FromBody] Contact item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveContactAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteContact([FromBody] Contact item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteContactAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }
        #endregion

        #region FraisSupplementaire
        [HttpPost("[action]")]
        public JsonResult FraisSupplementaires([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetFraisSupplementaires();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetFraisSupplementaires().Where(i => i.Nom.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<FraisSupplementaire>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<FraisSupplementaire>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveFraisSupplementaire([FromBody] FraisSupplementaire item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveFraisSupplementaireAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteFraisSupplementaire([FromBody] FraisSupplementaire item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteFraisSupplementaireAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }
        #endregion

        #region Salle
        [HttpPost("[action]")]
        public JsonResult Salles([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetSalles();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetSalles().Where(i => i.Nom.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Salle>(pagination, ref results);

                return Json(new
                {
                    Rows = results.ToList(),
                    FilteredTotalRows = results.Count(),
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Salle>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveSalle([FromBody] Salle item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveSalleAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteSalle([FromBody] Salle item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteSalleAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }
        #endregion

        #region Session
        [HttpPost("[action]")]
        public JsonResult Sessions([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetSessions();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetSessions().Where(i => i.Formation.Titre.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Session>(pagination, ref results);

                var rows = results.ToList();
                var filteredTotalRows = rows.Count();

                return Json(new
                {
                    Rows = rows,
                    FilteredTotalRows = filteredTotalRows,
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Session>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveSession([FromBody] Session item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveSessionAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteSession([FromBody] Session item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteSessionAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("[action]")]
        public IEnumerable<Contact> SearchContactsForEntreprise([FromQuery]string searchTerm, [FromQuery]Guid entrepriseId)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                var entreprise = _formationService.GetEntrepriseForId(entrepriseId);
                var contacts = entreprise.ContactEntreprises.Where(i => i.Contact.Nom.Contains(searchTerm) || i.Contact.Prenom.Contains(searchTerm))
                    .Select(i => i.Contact).ToList();
                return contacts;
            }
            else
            {
                return new List<Contact>();
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Formation> SearchFormations([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                var formations = _formationService.GetFormations()
                    .Where(i => i.Titre.Contains(searchTerm)).ToList();

                return formations;
            }
            else
            {
                return new List<Formation>();
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Formateur> SearchFormateursForFormation([FromQuery]string searchTerm, [FromQuery]Guid formationId)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                var formation = _formationService.GetFormationForId(formationId);
                var formateurs = formation.FormateurFormations.Where(i => i.Formateur.Nom.Contains(searchTerm) || i.Formateur.Prenom.Contains(searchTerm))
                    .Select(i => i.Formateur).ToList();
                return formateurs;
            }
            else
            {
                return new List<Formateur>();
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Salle> SearchSalles([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                var salles = _formationService.GetSalles()
                    .Where(i => i.Nom.Contains(searchTerm)).ToList();
                return salles;
            }
            else
            {
                return new List<Salle>();
            }
        }
        #endregion

        #region ParticipantSession
        [HttpPost("[action]")]
        public JsonResult Participants([FromBody]PaginationWithId pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetParticipantsForSessionId(pagination.Id);

                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetParticipantsForSessionId(pagination.Id).Where(i => i.Personne.Prenom.Contains(pagination.Search) || i.Personne.Nom.Contains(pagination.Search));
                int totalRows = results.Count();

                Util.Paginate<Participant>(
                    new Pagination
                    {
                        Descending = pagination.Descending,
                        Page = pagination.Page,
                        RowsPerPage = pagination.RowsPerPage,
                        Search = pagination.Search, SortBy = pagination.SortBy,
                        TotalItems = pagination.TotalItems
                    },
                    ref results);

                var rows = results.ToList();
                var filteredTotalRows = rows.Count();

                return Json(new
                {
                    Rows = rows,
                    FilteredTotalRows = filteredTotalRows,
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Participant>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveParticipant([FromBody] Participant item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveParticipantAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteParticipant([FromBody] Participant item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteParticipantAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("[action]")]
        public IEnumerable<Personne> SearchPersonnes([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                return _formationService.GetPersonnes().Where(i => i.Nom.Contains(searchTerm));
            }
            else
            {
                return new List<Personne>();
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Accessoire> SearchAccessoires([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                return _formationService.GetAccessoires().Where(i => i.Modele.Contains(searchTerm));
            }
            else
            {
                return new List<Accessoire>();
            }
        }
        #endregion

        #region FraisSupplementaireSession
        [HttpPost("[action]")]
        public JsonResult FraisSupplementaireSessions([FromBody]PaginationWithId pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetFraisSupplementaireSessionsForSessionId(pagination.Id);
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetFraisSupplementaireSessionsForSessionId(pagination.Id).Where(i => i.FraisSupplementaire.Nom.Contains(pagination.Search));
                int totalRows = results.Count();

                Util.Paginate<FraisSupplementaireSession>( new Pagination
                    {
                        Descending = pagination.Descending,
                        Page = pagination.Page,
                        RowsPerPage = pagination.RowsPerPage,
                        Search = pagination.Search,
                        SortBy = pagination.SortBy,
                        TotalItems = pagination.TotalItems
                    },
                    ref results);

                var rows = results.ToList();
                var filteredTotalRows = rows.Count();

                return Json(new
                {
                    Rows = rows,
                    FilteredTotalRows = filteredTotalRows,
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<FraisSupplementaireSession>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveFraisSupplementaireSession([FromBody] FraisSupplementaireSession item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.SaveFraisSupplementaireSessionAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteFraisSupplementaireSession([FromBody] FraisSupplementaireSession item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteFraisSupplementaireSessionAsync(item.SessionId, item.FraisSupplementaireId);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("[action]")]
        public IEnumerable<FraisSupplementaire> SearchFraisSupplementaires([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                return _formationService.GetFraisSupplementaires().Where(i => i.Nom.Contains(searchTerm));
            }
            else
            {
                return new List<FraisSupplementaire>();
            }
        }
        #endregion

        #region Facture
        [HttpPost("[action]")]
        public JsonResult Factures([FromBody]Pagination pagination)
        {
            if (pagination != null)
            {
                var results = _formationService.GetFactures();
                if (!String.IsNullOrEmpty(pagination.Search))
                    results = _formationService.GetFactures().Where(i => i.BonCommande.Contains(pagination.Search) || i.Entreprise.Nom.Contains(pagination.Search));
                int totalRows = results.Count();
                Util.Paginate<Facture>(pagination, ref results);

                var rows = results.ToList();
                var filteredTotalRows = rows.Count();

                return Json(new
                {
                    Rows = rows,
                    FilteredTotalRows = filteredTotalRows,
                    TotalRows = totalRows
                });
            }
            else
            {
                return Json(new
                {
                    Rows = new List<Facture>(),
                    FilteredTotalRows = 0,
                    TotalRows = 0
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveFacture([FromBody] Facture item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
            var result = await _formationService.SaveFactureAsync(item);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteFacture([FromBody] Facture item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formationService.DeleteFactureAsync(item.Id);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("[action]")]
        public IEnumerable<Session> SearchSessions([FromQuery]string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                return _formationService.GetSessions().Where(i => i.Entreprise.Nom.Contains(searchTerm) || i.Formation.Titre.Contains(searchTerm));
            }
            else
            {
                return new List<Session>();
            }
        }
        #endregion
    }
}
