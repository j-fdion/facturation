using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AppFactu.Data;
using Microsoft.EntityFrameworkCore;
using AppFactu.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using vueAppFactu.Models;
using vueAppFactu.Extensions;

namespace AppFactu.Services
{
    public class GestionService : IGestionService
    {
        private readonly ApplicationDbContext _context;

        public GestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Get
        public IQueryable<Personne> GetPersonnes()
        {
            var personnes = _context.Personnes.Include("Entreprise");
            return personnes.AsNoTracking();
        }

        public IQueryable<Personne> GetPersonnesForEntreprise(Guid entrepriseId)
        {
            var personnes = _context.Personnes.Include("Entreprise").Where(i => i.Entreprise.Id == entrepriseId);
            return personnes.AsNoTracking();
        }

        public IQueryable<Formation> GetFormations()
        {
            var formations = _context.Formations;
            return formations.AsNoTracking();
        }

        public IQueryable<Formateur> GetFormateurs()
        {
            var formateurs = _context.Formateurs.Include(i => i.FormateurFormations).ThenInclude(i => i.Formation);
            return formateurs.AsNoTracking();
        }

        public IQueryable<Formateur> GetPagedFormateurs(int skip, int take)
        {
            var formateurs = _context.Formateurs.Skip(skip).Take(take).Include("FormateurFormations.Formation");
            return formateurs.AsNoTracking();
        }

        public IQueryable<Session> GetSessions()
        {
            var sessions = _context.Sessions
                .Include("Formateur")
                .Include("Formation")
                .Include("Salle")
                .Include("Contact")
                .Include("Entreprise")
                .Include(i => i.PersonneSessions).ThenInclude(j => j.Personne).ThenInclude(k => k.Entreprise)
                .Include(i => i.ParticipantSessions).ThenInclude(j => j.Participant).ThenInclude(k => k.Personne);
            return sessions.AsNoTracking();
        }

        public IQueryable<Session> GetSessionsBetweenDates(DateTimeOffset start, DateTimeOffset end)
        {
            var sessions = _context.Sessions.Include("Formateur")
                .Include("Entreprise")
                .Include("Formation")
                .Include("Salle")
                .Include(i => i.Contact)
                .Include(i => i.PersonneSessions)
                .ThenInclude(j => j.Personne)
                .ThenInclude(k => k.Entreprise)
                .Include(i => i.ParticipantSessions)
                .Where(k => k.Date >= start && k.Date <= end);
            return sessions.AsNoTracking();
        }

        public IQueryable<Entreprise> GetEntreprises()
        {
            var entreprises = _context.Entreprises.Include(i => i.ContactEntreprises).ThenInclude(i => i.Contact);
            return entreprises.AsNoTracking();
        }

        public IQueryable<Contact> GetContacts()
        {
            var contacts = _context.Contacts;
            return contacts.AsNoTracking();
        }

        public IQueryable<PersonneSession> GetPersonneSessions()
        {
            var personneSessions = _context.PersonneSessions.Include(i => i.Personne).Include(i => i.Session);
            return personneSessions.AsNoTracking();
        }

        public IQueryable<Salle> GetSalles()
        {
            var salles = _context.Salles;
            return salles.AsNoTracking();
        }

        public IQueryable<FraisSupplementaire> GetFraisSupplementaires()
        {
            var fraisSupplementaires = _context.FraisSupplementaires;
            return fraisSupplementaires.AsNoTracking();
        }

        public IQueryable<Facture> GetFactures()
        {
            var factures = _context.Factures.Include(i => i.Entreprise).Include(i => i.Sessions);
            return factures.AsNoTracking();
        }

        public IQueryable<Accessoire> GetAccessoires()
        {
            var accessoires = _context.Accessoires.Include("TypeAccessoire");
            return accessoires.AsNoTracking();
        }

        public IQueryable<TypeAccessoire> GetTypeAccessoires()
        {
            var typeAccessoires = _context.TypeAccessoires;
            return typeAccessoires.AsNoTracking();
        }
        #endregion

        #region GetForId
        public Personne GetPersonneForId(Guid personneId)
        {
            var personne = _context.Personnes.Where(i => i.Id == personneId)
                .Include(i => i.Entreprise)
                .Include(i => i.PersonneSessions)
                .ThenInclude(i => i.Session)
                .SingleOrDefault();
            return personne;
        }

        public Formation GetFormationForId(Guid formationId)
        {
            var formation = _context.Formations.Where(i => i.Id == formationId)
                .Include("FormateurFormations.Formateur")
                .SingleOrDefault();
            return formation;
        }

        public Formateur GetFormateurForId(Guid formateurId)
        {
            var formateur = _context.Formateurs.Where(i => i.Id == formateurId)
                .Include("FormateurFormations.Formation")
                .SingleOrDefault();
            return formateur;
        }

        public Session GetSessionForId(Guid sessionId)
        {
            var session = _context.Sessions.Where(i => i.Id == sessionId)
                .Include(i => i.Formateur)
                .Include(i => i.Formation)
                .Include(i => i.Entreprise)
                .Include(i => i.Contact)
                .Include(i => i.Salle)
                .Include(i => i.PersonneSessions)
                .ThenInclude(i => i.Personne)
                .ThenInclude(i => i.Entreprise)
                .Include(i => i.ParticipantSessions)
                .ThenInclude(i => i.Participant)
                .ThenInclude(i => i.Personne)
                .ThenInclude(i => i.Entreprise)
                .Include("FraisSupplementaireSessions.FraisSupplementaire")
                .SingleOrDefault();
            return session;
        }
        public List<Session> GetSessionsForPersonneId(Guid personneId)
        {
            var sessions = _context.ParticipantSessions.Where(i => i.Participant.Personne.Id == personneId).Select(i => i.Session)
                .Include(i => i.Formateur)
                .Include(i => i.Formation)
                .Include(i => i.Salle)
                .Include(i => i.Contact)
                .Include("FraisSupplementaireSessions.FraisSupplementaire")
                .Include(i => i.ParticipantSessions)
                .ThenInclude(i => i.Participant)
                .ThenInclude(i => i.Personne)
                .ThenInclude(i => i.Entreprise);
            return sessions.ToList();
        }

        public Entreprise GetEntrepriseForId(Guid entrepriseId)
        {
            var entreprise = _context.Entreprises.Where(i => i.Id == entrepriseId)
                .Include(i => i.ContactEntreprises)
                .ThenInclude(i => i.Contact)
                .SingleOrDefault();
            return entreprise;
        }

        public Contact GetContactForId(Guid contactId)
        {
            var contact = _context.Contacts.Where(i => i.Id == contactId).SingleOrDefault();
            return contact;
        }

        public ContactEntreprise GetContactEntrepriseForId(Guid contactId, Guid entrepriseId)
        {
            var contactEntreprise = _context.ContactEntreprises.Where(i => i.ContactId == contactId && i.EntrepriseId == entrepriseId)
                .Include(i => i.Contact)
                .Include(i => i.Entreprise)
                .SingleOrDefault();
            return contactEntreprise;
        }

        public PersonneSession GetPersonneSessionForId(Guid personneId, Guid sessionId)
        {
            var personneSession = _context.PersonneSessions.Where(i => i.PersonneId == personneId && i.SessionId == sessionId)
                .Include(i => i.Personne)
                .Include(i => i.Session)
                .SingleOrDefault();
            return personneSession;
        }

        public List<PersonneSession> GetPersonneSessionForId(Guid sessionId)
        {
            var personneSession = _context.PersonneSessions.Where(i => i.SessionId == sessionId)
                .Include(i => i.Personne)
                .Include(i => i.Session);
            return personneSession.ToList();
        }

        public FormateurFormation GetFormateurFormationForId(Guid formateurId, Guid formationId)
        {
            var formateurFormation = _context.FormateurFormations.Where(i => i.FormateurId == formateurId && i.FormationId == formationId)
                .Include(i => i.Formateur)
                .Include(i => i.Formation)
                .SingleOrDefault();
            return formateurFormation;
        }

        public List<FormateurFormation> GetFormateurFormationForId(Guid formateurId)
        {
            var formateurFormation = _context.FormateurFormations.Where(i => i.FormateurId == formateurId)
                .Include(i => i.Formateur)
                .Include(i => i.Formation);
            return formateurFormation.ToList();
        }

        public Salle GetSalleForId(Guid salleId)
        {
            var salle = _context.Salles.Where(i => i.Id == salleId).SingleOrDefault();
            return salle;
        }

        public FraisSupplementaireSession GetFraisSupplementaireSessionForId(Guid fraisSupplementaireId, Guid sessionId)
        {
            var fs = _context.FraisSupplementaireSessions.Where(i => i.FraisSupplementaireId == fraisSupplementaireId && i.SessionId == sessionId)
                .Include(i => i.FraisSupplementaire)
                .Include(i => i.Session)
                .SingleOrDefault();
            return fs;
        }

        public FraisSupplementaire GetFraisSupplementaireForId(Guid fraisId)
        {
            var fraisSupplementaire = _context.FraisSupplementaires.Where(i => i.Id == fraisId).SingleOrDefault();
            return fraisSupplementaire;
        }

        public Facture GetFactureForId(Guid sessionId)
        {
            var facture = _context.Factures.Where(i => i.Id == sessionId)
                .Include("Sessions.PersonneSessions.Personne.Entreprise")
                .Include("Sessions.ParticipantSessions.Participant.Personne.Entreprise")
                .Include("Sessions.ParticipantSessions.Participant.Accessoire.TypeAccessoire")
                .Include("Sessions.FraisSupplementaireSessions.FraisSupplementaire")
                .Include("Entreprise.ContactEntreprises")
                .Include(i => i.Entreprise)
                .Include(i => i.Sessions)
                .ThenInclude(i => i.Formation)
                .SingleOrDefault();
            return facture;
        }

        public Accessoire GetAccessoireForId(Guid id)
        {
            var accessoire = _context.Accessoires.Where(i => i.Id == id)
                .Include(i => i.TypeAccessoire).SingleOrDefault();
            return accessoire;
        }

        public TypeAccessoire GetTypeAccessoireForId(Guid id)
        {
            var typeAccessoire = _context.TypeAccessoires.Where(i => i.Id == id).SingleOrDefault();
            return typeAccessoire;
        }

        public Participant GetParticipantForId(Guid id)
        {
            var participant = _context.Participants.Where(i => i.Id == id)
                    .Include(i => i.Accessoire)
                    .Include(i => i.Personne)
                    .Include(i => i.ParticipantSessions)
                    .Include("ParticipantSessions.Personne.Entreprise")
                    .SingleOrDefault();
            return participant;
        }

        public ParticipantSession GetParticipantSessionForId(Guid participantId, Guid sessionId)
        {
            var participantSession = _context.ParticipantSessions.Where(i => i.ParticipantId == participantId && i.SessionId == sessionId)
            .Include(i => i.Participant)
            .Include(i => i.Session)
            .SingleOrDefault();

            return participantSession;
        }

        public List<ParticipantSession> GetParticipantSessionForId(Guid sessionId)
        {
            var participantSessions = _context.ParticipantSessions.Where(i => i.SessionId == sessionId)
                .Include(i => i.Participant)
                .ThenInclude(i => i.Accessoire)
                .Include(i => i.Participant)
                .ThenInclude(i => i.Personne)
                .Include(i => i.Session);
            return participantSessions.ToList();
        }

        public ParticipantSession GetParticipantSessionForPersonneIdAndSessionId(Guid personneId, Guid sessionId)
        {
            var participantSession = _context.ParticipantSessions.Where(i => i.Participant.Personne.Id == personneId && i.SessionId == sessionId)
            .Include(i => i.Participant)
            .ThenInclude(i => i.Personne)
            .ThenInclude(i => i.Entreprise)
            .Include(i => i.Session)
            .SingleOrDefault();

            return participantSession;
        }

        public IQueryable<Participant> GetParticipantsForSessionId(Guid sessionId)
        {
            var participants = _context.Participants.Where(i => i.ParticipantSessions.Any(j => j.SessionId == sessionId))
                .Include("ParticipantSessions.Participant.Personne.Entreprise")
                .Include("ParticipantSessions.Participant.Accessoire.TypeAccessoire");

            return participants;
        }

        public IQueryable<FraisSupplementaireSession> GetFraisSupplementaireSessionsForSessionId(Guid sessionId)
        {
            var fraisSupplementaireSession = _context.FraisSupplementaireSessions.Where(i => i.SessionId == sessionId)
                .Include("FraisSupplementaire");

            return fraisSupplementaireSession;
        }
        #endregion

        #region AddAsync

        /*public async Task<bool> AddSessionAsync(NewSession newSession)
        {
            var formation = _context.Formations.FirstOrDefault(i => i.Id == newSession.Formation);
            var formateur = _context.Formateurs.FirstOrDefault(i => i.Id == newSession.Formateur);
            var salle = _context.Salles.FirstOrDefault(i => i.Id == newSession.Salle);
            var entreprise = _context.Entreprises.FirstOrDefault(i => i.Id == newSession.Entreprise);
            var contact = _context.Contacts.FirstOrDefault(i => i.Id == newSession.Contact);

            var session = new Session
            {
                Id = Guid.NewGuid(),
                EntrepriseId = newSession.Entreprise,
                Entreprise = entreprise,
                ContactId = newSession.Contact,
                Contact = contact,
                Formation = formation,
                Formateur = formateur,
                Salle = salle,
                Date = newSession.Date,
                TypeFacturation = newSession.TypeFacturation,
                UtiliseDureeSession = newSession.UtiliseDureeSession,
                Duree = newSession.Duree,
                UtilisePrixSession = newSession.UtilisePrixSession,
                Prix = newSession.Prix,
                NombrePersonnesAttendues = newSession.NombrePersonnesAttendues,
                BonCommande = newSession.BonCommande,
                DateCreation = DateTimeOffset.UtcNow,
                DateModification = DateTimeOffset.UtcNow
            };

            int changes = 0;
            #region ParticipantSession
            var participantSessions = new List<ParticipantSession>();
            if (newSession.NewParticipants != null)
            {
                foreach (var newParticipant in newSession.NewParticipants)
                {

                    newParticipant.Id = Guid.NewGuid();
                    Accessoire accessoire = null;
                    Guid accessoireId = Guid.Empty;
                    if (Guid.TryParse(newParticipant.Accessoire.Val, out accessoireId))
                    {
                        accessoire = GetAccessoireForId(accessoireId);
                    }
                    Personne personne = null;
                    Guid personneId = Guid.Empty;
                    if (Guid.TryParse(newParticipant.Personne.Val, out personneId))
                    {
                        personne = GetPersonneForId(personneId);
                    }

                    var participantSession = new ParticipantSession
                    {
                        ParticipantId = newParticipant.Id,
                        Participant = new Participant
                        {
                            Id = newParticipant.Id,
                            Absence = newParticipant.Absence,
                            Accessoire = accessoire,
                            AccessoireSeulement = newParticipant.AccessoireSeulement,
                            Personne = personne,
                            Grandeur = newParticipant.Grandeur,
                            DateModification = DateTimeOffset.UtcNow,
                            DateCreation = DateTimeOffset.UtcNow
                        },
                        SessionId = session.Id,
                        Session = session
                    };
                    //Création de l'entité ParticipantSession + l'ajout dans la liste
                    changes += 2;
                    participantSessions.Add(participantSession);
                }
            }
            #endregion

            var fraisSupplementaires = new List<FraisSupplementaireSession>();
            if (newSession.NewFraisSupplementaires != null)
            {
                foreach (var fs in newSession.NewFraisSupplementaires)
                {
                    var fraisSupplementaire = GetFraisSupplementaireForId(fs.FraisSupplementaireId);
                    fraisSupplementaires.Add(new FraisSupplementaireSession
                    {
                        FraisSupplementaireId = fs.FraisSupplementaireId,
                        FraisSupplementaire = fraisSupplementaire,
                        SessionId = session.Id,
                        Session = session,
                        Quantite = fs.Quantite
                    });
                    changes++;
                }
            }

            session.FraisSupplementaireSessions = fraisSupplementaires;
            session.ParticipantSessions = participantSessions;

            _context.Sessions.Add(session);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == (changes + 1);
        }

        public async Task<bool> AddSalleAsync(NewSalle newSalle)
        {
            var salle = new Salle
            {
                Id = Guid.NewGuid(),
                Nom = newSalle.Nom,
                DateCreation = DateTimeOffset.UtcNow,
                DateModification = DateTimeOffset.UtcNow
            };

            _context.Salles.Add(salle);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> AddFactureAsync(NewFacture newFacture)
        {
            int changes = 0;
            var sessions = new List<Session>();
            if (newFacture.SessionIds != null)
            {
                foreach (var id in newFacture.SessionIds)
                {
                    sessions.Add(GetSessionForId(id));
                    changes++;
                }
            }

            var entreprise = _context.Entreprises.FirstOrDefault(i => i.Id == newFacture.EntrepriseId);

            var facture = new Facture
            {
                Entreprise = entreprise,
                Sessions = sessions,
                BonCommande = newFacture.BonCommande,
                Date = newFacture.Date,
                PeriodeDebut = newFacture.PeriodeDebut,
                PeriodeFin = newFacture.PeriodeFin,
                DateCreation = DateTimeOffset.UtcNow,
                DateModification = DateTimeOffset.UtcNow
            };

            _context.Factures.Add(facture);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == (changes + 1);
        }*/

        #endregion

        #region SaveAsync
        public async Task<Personne> SavePersonneAsync(Personne personne)
        {
            var existingPersonne = _context.Personnes
             .Include(c => c.Entreprise)
             .SingleOrDefault(c => c.Id == personne.Id);

            var newEntreprise = _context.Entreprises.SingleOrDefault(c => c.Id == personne.Entreprise.Id);
            if (newEntreprise == null)
                return null;

            if (existingPersonne == null)
            {
                existingPersonne = new Personne
                {
                    Id = personne.Id,
                    Nom = personne.Nom,
                    Prenom = personne.Prenom,
                    DateNaissance = personne.DateNaissance,
                    Entreprise = newEntreprise,
                    Commentaire = personne.Commentaire,
                    DateModification = DateTimeOffset.UtcNow,
                    DateCreation = DateTimeOffset.UtcNow
                };

                _context.Personnes.Add(existingPersonne);
            }
            else
            {
                existingPersonne.Nom = personne.Nom;
                existingPersonne.Prenom = personne.Prenom;
                existingPersonne.DateNaissance = personne.DateNaissance;
                existingPersonne.Entreprise = newEntreprise;
                existingPersonne.Commentaire = personne.Commentaire;
                existingPersonne.DateModification = DateTimeOffset.UtcNow;

                _context.Entry(existingPersonne).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingPersonne;
            else
                return null;
        }

        public async Task<Formateur> SaveFormateurAsync(Formateur newFormateur)
        {
            var existingFormateur = _context.Formateurs
                 .Include(i => i.FormateurFormations)
                 .SingleOrDefault(c => c.Id == newFormateur.Id);

            if (existingFormateur == null)
            {
                existingFormateur = new Formateur
                {
                    Id = newFormateur.Id,
                    Nom = newFormateur.Nom,
                    Prenom = newFormateur.Prenom,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                var formateurFormations = newFormateur.FormateurFormations.Select(ff =>
                {
                    var f = GetFormateurFormationForId(newFormateur.Id, ff.FormationId);
                    if (f == null)
                    {
                        return new FormateurFormation
                        {
                            FormateurId = newFormateur.Id,
                            Formateur = existingFormateur,
                            FormationId = ff.FormationId,
                            Formation = GetFormationForId(ff.FormationId)
                        };
                    }
                    else
                    {
                        return f;
                    }
                }).ToList();

                existingFormateur.FormateurFormations = formateurFormations;
                _context.Formateurs.Add(existingFormateur);
            }
            else
            {
                var ids = newFormateur.FormateurFormations.Select(ff => ff.FormationId).ToList();

                foreach (var ff in existingFormateur.FormateurFormations.ToList())
                {
                    // Remove the roles which are not in the list of new roles
                    if (!ids.Contains(ff.FormationId))
                        existingFormateur.FormateurFormations.Remove(ff);
                    // Removes role 3 in the example
                }

                foreach (var id in ids)
                {
                    // Add the roles which are not in the list of user's roles
                    if (!existingFormateur.FormateurFormations.Any(r => r.FormationId == id))
                    {
                        var newFormateurFormation = new FormateurFormation { FormationId = id };
                        _context.FormateurFormations.Attach(newFormateurFormation);
                        existingFormateur.FormateurFormations.Add(newFormateurFormation);
                    }
                    // Adds roles 1 and 2 in the example
                }

                existingFormateur.Nom = newFormateur.Nom;
                existingFormateur.Prenom = newFormateur.Prenom;
                existingFormateur.DateModification = DateTimeOffset.UtcNow;

                //_context.Entry(existingFormateur).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingFormateur;
            else
                return null;
        }

        public async Task<Entreprise> SaveEntrepriseAsync(Entreprise newEntreprise)
        {
            var existingEntreprise = _context.Entreprises
                             .Include(c => c.ContactEntreprises)
                             .ThenInclude(c => c.Contact)
                             .SingleOrDefault(c => c.Id == newEntreprise.Id);

            if (existingEntreprise == null)
            {
                existingEntreprise = new Entreprise
                {
                    Id = newEntreprise.Id,
                    Nom = newEntreprise.Nom,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                var contactEntreprises = newEntreprise.ContactEntreprises.Select(ce =>
                {
                    var c = GetContactEntrepriseForId(ce.ContactId, newEntreprise.Id);
                    if (c == null)
                    {
                        return new ContactEntreprise
                        {
                            ContactId = ce.ContactId,
                            Contact = GetContactForId(ce.ContactId),
                            EntrepriseId = newEntreprise.Id,
                            Entreprise = existingEntreprise
                        };
                    }
                    else
                    {
                        return c;
                    }
                }).ToList();

                existingEntreprise.ContactEntreprises = contactEntreprises;

                _context.Entreprises.Add(existingEntreprise);
            }
            else
            {
                var ids = newEntreprise.ContactEntreprises.Select(ff => ff.ContactId).ToList();

                foreach (var ff in existingEntreprise.ContactEntreprises.ToList())
                {
                    // Remove the roles which are not in the list of new roles
                    if (!ids.Contains(ff.ContactId))
                        existingEntreprise.ContactEntreprises.Remove(ff);
                    // Removes role 3 in the example
                }

                foreach (var id in ids)
                {
                    // Add the roles which are not in the list of user's roles
                    if (!existingEntreprise.ContactEntreprises.Any(r => r.ContactId == id))
                    {
                        var newContactEntreprise = new ContactEntreprise { ContactId = id };
                        _context.ContactEntreprises.Attach(newContactEntreprise);
                        existingEntreprise.ContactEntreprises.Add(newContactEntreprise);
                    }
                    // Adds roles 1 and 2 in the example
                }

                existingEntreprise.Nom = newEntreprise.Nom;
                existingEntreprise.DateModification = DateTimeOffset.UtcNow;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingEntreprise;
            else
                return null;
        }

        public async Task<TypeAccessoire> SaveTypeAccessoireAsync(TypeAccessoire typeAccessoire)
        {
            var existingTypeAccessoire = _context.TypeAccessoires
             .SingleOrDefault(c => c.Id == typeAccessoire.Id);

            if (existingTypeAccessoire == null)
            {
                existingTypeAccessoire = new TypeAccessoire
                {
                    Id = typeAccessoire.Id,
                    Nom = typeAccessoire.Nom,
                    DateModification = DateTimeOffset.UtcNow,
                    DateCreation = DateTimeOffset.UtcNow
                };

                _context.TypeAccessoires.Add(existingTypeAccessoire);
            }
            else
            {
                existingTypeAccessoire.Nom = typeAccessoire.Nom;
                existingTypeAccessoire.DateModification = DateTimeOffset.UtcNow;

                _context.Entry(existingTypeAccessoire).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingTypeAccessoire;
            else
                return null;
        }

        public async Task<Accessoire> SaveAccessoireAsync(Accessoire accessoire)
        {
            var existingAccessoire = _context.Accessoires
                 .Include(c => c.TypeAccessoire)
                 .SingleOrDefault(c => c.Id == accessoire.Id);

            var typeAccessoire = _context.TypeAccessoires.SingleOrDefault(c => c.Id == accessoire.TypeAccessoire.Id);

            if (typeAccessoire == null)
                return null;

            if (existingAccessoire == null)
            {
                existingAccessoire = new Accessoire
                {
                    Id = Guid.NewGuid(),
                    Modele = accessoire.Modele,
                    Prix = accessoire.Prix,
                    TypeAccessoire = typeAccessoire,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                _context.Accessoires.Add(existingAccessoire);
            }
            else
            {
                existingAccessoire.Modele = accessoire.Modele;
                existingAccessoire.Prix = accessoire.Prix;
                existingAccessoire.TypeAccessoire = typeAccessoire;
                existingAccessoire.DateModification = DateTimeOffset.UtcNow;

                _context.Entry(existingAccessoire).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingAccessoire;
            else
                return null;
        }

        public async Task<Formation> SaveFormationAsync(Formation formation)
        {
            var existingFormation = await _context.Formations.SingleOrDefaultAsync(c => c.Id == formation.Id);
            if (existingFormation == null)
            {
                existingFormation = new Formation
                {
                    Id = Guid.NewGuid(),
                    Titre = formation.Titre,
                    Duree = formation.Duree,
                    TauxHoraire = formation.TauxHoraire,
                    PrixForfaitaire = formation.PrixForfaitaire,
                    PrixUnitaire = formation.PrixUnitaire,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                _context.Formations.Add(existingFormation);
            }
            else
            {
                existingFormation.Titre = formation.Titre;
                existingFormation.Duree = formation.Duree;
                existingFormation.TauxHoraire = formation.TauxHoraire;
                existingFormation.PrixForfaitaire = formation.PrixForfaitaire;
                existingFormation.PrixUnitaire = formation.PrixUnitaire;
                existingFormation.DateModification = DateTimeOffset.UtcNow;

                _context.Entry(existingFormation).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingFormation;
            else
                return null;
        }

        public async Task<Contact> SaveContactAsync(Contact contact)
        {
            var existingContact = await _context.Contacts.SingleOrDefaultAsync(c => c.Id == contact.Id);

            if (existingContact == null)
            {
                existingContact = new Contact
                {
                    Id = Guid.NewGuid(),
                    Nom = contact.Nom,
                    Prenom = contact.Prenom,
                    NoTelephone1 = contact.NoTelephone1,
                    NoTelephone2 = contact.NoTelephone2,
                    Email = contact.Email,
                    Commentaire = contact.Commentaire,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                _context.Contacts.Add(existingContact);
            }
            else
            {
                existingContact.Nom = contact.Nom;
                existingContact.Prenom = contact.Prenom;
                existingContact.NoTelephone1 = contact.NoTelephone1;
                existingContact.NoTelephone2 = contact.NoTelephone2;
                existingContact.Email = contact.Email;
                existingContact.Commentaire = contact.Commentaire;
                existingContact.DateModification = DateTimeOffset.UtcNow;

                _context.Entry(existingContact).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingContact;
            else
                return null;
        }

        public async Task<FraisSupplementaire> SaveFraisSupplementaireAsync(FraisSupplementaire frais)
        {
            var existingFrais = await _context.FraisSupplementaires.SingleOrDefaultAsync(c => c.Id == frais.Id);

            if (existingFrais == null)
            {
                existingFrais = new FraisSupplementaire
                {
                    Id = Guid.NewGuid(),
                    Nom = frais.Nom,
                    CoutUnite = frais.CoutUnite,
                    NomQuantite = frais.NomQuantite,
                    NomUnite = frais.NomUnite,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                _context.FraisSupplementaires.Add(existingFrais);
            }
            else
            {
                existingFrais.Nom = frais.Nom;
                existingFrais.CoutUnite = frais.CoutUnite;
                existingFrais.NomQuantite = frais.NomQuantite;
                existingFrais.NomUnite = frais.NomUnite;
                existingFrais.DateModification = DateTimeOffset.UtcNow;

                _context.Entry(existingFrais).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingFrais;
            else
                return null;
        }

        public async Task<Salle> SaveSalleAsync(Salle salle)
        {
            var existingSalle = await _context.Salles.SingleOrDefaultAsync(c => c.Id == salle.Id);

            if (existingSalle == null)
            {
                existingSalle = new Salle
                {
                    Id = Guid.NewGuid(),
                    Nom = salle.Nom,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                _context.Salles.Add(existingSalle);
            }
            else
            {
                existingSalle.Nom = salle.Nom;
                existingSalle.DateModification = DateTimeOffset.UtcNow;

                _context.Entry(existingSalle).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingSalle;
            else
                return null;
        }

        public async Task<Session> SaveSessionAsync(Session newSession)
        {
            var existingSession = _context.Sessions
                 .Include(c => c.Formation)
                 .Include(c => c.Formateur)
                 .Include(c => c.Salle)
                 .Include(c => c.PersonneSessions)
                 .ThenInclude(c => c.Personne)
                 .Include(c => c.ParticipantSessions)
                 .SingleOrDefault(c => c.Id == newSession.Id);

            if (existingSession == null)
            {
                var formation = _context.Formations.FirstOrDefault(i => i.Id == newSession.Formation.Id);
                var formateur = _context.Formateurs.FirstOrDefault(i => i.Id == newSession.Formateur.Id);
                var salle = _context.Salles.FirstOrDefault(i => i.Id == newSession.Salle.Id);
                var entreprise = _context.Entreprises.FirstOrDefault(i => i.Id == newSession.Entreprise.Id);
                var contact = _context.Contacts.FirstOrDefault(i => i.Id == newSession.Contact.Id);

                existingSession = new Session
                {
                    Id = Guid.NewGuid(),
                    EntrepriseId = newSession.Entreprise.Id,
                    Entreprise = entreprise,
                    ContactId = newSession.Contact.Id,
                    Contact = contact,
                    FormationId = newSession.Formation.Id,
                    Formation = formation,
                    FormateurId = newSession.Formateur.Id,
                    Formateur = formateur,
                    SalleId = newSession.Salle.Id,
                    Salle = salle,
                    Date = newSession.Date,
                    TypeFacturation = newSession.TypeFacturation,
                    UtiliseDureeSession = newSession.UtiliseDureeSession,
                    Duree = newSession.Duree,
                    UtilisePrixSession = newSession.UtilisePrixSession,
                    Prix = newSession.Prix,
                    NombrePersonnesAttendues = newSession.NombrePersonnesAttendues,
                    BonCommande = newSession.BonCommande,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                /*var fraisSupplementaireSessions = newSession.FraisSupplementaireSessions.Select(ff =>
                {
                    var f = GetFraisSupplementaireSessionForId(ff.FraisSupplementaireId, newSession.Id);
                    if (f == null)
                    {
                        return new FraisSupplementaireSession
                        {
                            SessionId = newSession.Id,
                            Session = newSession,
                            FraisSupplementaireId = ff.FraisSupplementaireId,
                            FraisSupplementaire = GetFraisSupplementaireForId(ff.FraisSupplementaireId)
                        };
                    }
                    else
                    {
                        return f;
                    }
                }).ToList();

                var participantSessions = newSession.ParticipantSessions.Select(ff =>
                {
                    var f = GetParticipantSessionForId(ff.ParticipantId, newSession.Id);
                    if (f == null)
                    {
                        return new ParticipantSession
                        {
                            ParticipantId = ff.ParticipantId,
                            Participant = GetParticipantForId(ff.ParticipantId),
                            SessionId = newSession.Id,
                            Session = newSession
                        };
                    }
                    else
                    {
                        return f;
                    }
                }).ToList();

                existingSession.FraisSupplementaireSessions = fraisSupplementaireSessions;
                existingSession.ParticipantSessions = participantSessions;*/

                _context.Sessions.Add(existingSession);
            }
            else
            {
                /*var ids = newSession.FraisSupplementaireSessions.Select(ff => ff.FraisSupplementaireId).ToList();

                foreach (var ff in existingSession.FraisSupplementaireSessions.ToList())
                {
                    if (!ids.Contains(ff.FraisSupplementaireId))
                        existingSession.FraisSupplementaireSessions.Remove(ff);
                }

                foreach (var id in ids)
                {
                    if (!existingSession.FraisSupplementaireSessions.Any(r => r.FraisSupplementaireId == id))
                    {
                        var newFraisSupplementaireSession = new FraisSupplementaireSession { FraisSupplementaireId = id };
                        _context.FraisSupplementaireSessions.Attach(newFraisSupplementaireSession);
                        existingSession.FraisSupplementaireSessions.Add(newFraisSupplementaireSession);
                    }
                }

                ids = newSession.ParticipantSessions.Select(ff => ff.ParticipantId).ToList();

                foreach (var ff in existingSession.ParticipantSessions.ToList())
                {
                    if (!ids.Contains(ff.ParticipantId))
                        existingSession.ParticipantSessions.Remove(ff);
                }

                foreach (var id in ids)
                {
                    if (!existingSession.ParticipantSessions.Any(r => r.ParticipantId == id))
                    {
                        var newParticipantSessions = new ParticipantSession { ParticipantId = id };
                        _context.ParticipantSessions.Attach(newParticipantSessions);
                        existingSession.ParticipantSessions.Add(newParticipantSessions);
                    }
                }*/

                var formation = _context.Formations.FirstOrDefault(i => i.Id == newSession.Formation.Id);
                var formateur = _context.Formateurs.FirstOrDefault(i => i.Id == newSession.Formateur.Id);
                var salle = _context.Salles.FirstOrDefault(i => i.Id == newSession.Salle.Id);
                var entreprise = _context.Entreprises.FirstOrDefault(i => i.Id == newSession.Entreprise.Id);
                var contact = _context.Contacts.FirstOrDefault(i => i.Id == newSession.Contact.Id);

                existingSession.EntrepriseId = newSession.Entreprise.Id;
                existingSession.Entreprise = entreprise;
                existingSession.ContactId = newSession.Contact.Id;
                existingSession.Contact = contact;
                existingSession.FormationId = newSession.Formation.Id;
                existingSession.Formation = formation;
                existingSession.FormateurId = newSession.Formateur.Id;
                existingSession.Formateur = formateur;
                existingSession.SalleId = newSession.Salle.Id;
                existingSession.Salle = salle;
                existingSession.Date = newSession.Date;
                existingSession.TypeFacturation = newSession.TypeFacturation;
                existingSession.UtiliseDureeSession = newSession.UtiliseDureeSession;
                existingSession.Duree = newSession.Duree;
                existingSession.UtilisePrixSession = newSession.UtilisePrixSession;
                existingSession.Prix = newSession.Prix;
                existingSession.NombrePersonnesAttendues = newSession.NombrePersonnesAttendues;
                existingSession.BonCommande = newSession.BonCommande;
                existingSession.DateCreation = DateTimeOffset.UtcNow;
                existingSession.DateModification = DateTimeOffset.UtcNow;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingSession;
            else
                return null;
        }

        public async Task<Participant> SaveParticipantAsync(Participant participant)
        {
            var existingParticipant = _context.Participants
                .Include(i => i.Personne)
                .Include(i => i.Accessoire)
                .Include(i => i.ParticipantSessions)
                 .SingleOrDefault(c => c.Id == participant.Id);

            if (existingParticipant == null)
            {
                var personne = _context.Personnes.FirstOrDefault(i => i.Id == participant.Personne.Id);
                var accessoire = _context.Accessoires.FirstOrDefault(i => i.Id == participant.Accessoire.Id);
                var session = _context.Sessions.Include(i=> i.ParticipantSessions).FirstOrDefault(i => i.Id == participant.SessionId);

                existingParticipant = new Participant
                {
                    Id = Guid.NewGuid(),
                    Personne = personne,
                    Accessoire = accessoire,
                    Absence = participant.Absence,
                    AccessoireSeulement = participant.AccessoireSeulement,
                    Grandeur = participant.Grandeur,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                if (session.ParticipantSessions == null)
                    session.ParticipantSessions = new List<ParticipantSession>();

                session.ParticipantSessions.Add(new ParticipantSession
                {
                    ParticipantId = existingParticipant.Id,
                    Participant = existingParticipant,
                    SessionId = session.Id,
                    Session = session
                });

                _context.Participants.Add(existingParticipant);
            }
            else
            {

                var personne = _context.Personnes.FirstOrDefault(i => i.Id == participant.Personne.Id);
                var accessoire = _context.Accessoires.FirstOrDefault(i => i.Id == participant.Accessoire.Id);

                existingParticipant.Personne = personne;
                existingParticipant.Accessoire = accessoire;
                existingParticipant.Absence = participant.Absence;
                existingParticipant.AccessoireSeulement = participant.AccessoireSeulement;
                existingParticipant.Grandeur = participant.Grandeur;
                existingParticipant.DateModification = DateTimeOffset.UtcNow;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingParticipant;
            else
                return null;
        }

        public async Task<FraisSupplementaireSession> SaveFraisSupplementaireSessionAsync(FraisSupplementaireSession fraisSupplementaireSession)
        {
            var existing = _context.FraisSupplementaireSessions
                .Include(i => i.FraisSupplementaire)
                .Include(i => i.Session)
                 .SingleOrDefault(c => c.SessionId == fraisSupplementaireSession.SessionId
                 && c.FraisSupplementaireId == fraisSupplementaireSession.FraisSupplementaireId);

            if (existing == null)
            {
                var fs = _context.FraisSupplementaires.FirstOrDefault(i => i.Id == fraisSupplementaireSession.FraisSupplementaireId);
                var session = _context.Sessions.Include(i => i.FraisSupplementaireSessions).FirstOrDefault(i => i.Id == fraisSupplementaireSession.SessionId);

                existing = new FraisSupplementaireSession
                {
                    SessionId = fraisSupplementaireSession.SessionId,
                    Session = session,
                    FraisSupplementaireId = fraisSupplementaireSession.FraisSupplementaireId,
                    FraisSupplementaire = fs,
                    Quantite = fraisSupplementaireSession.Quantite,
                    DateModification = DateTime.UtcNow,
                    DateCreation = DateTimeOffset.UtcNow
                };

                if (session.FraisSupplementaireSessions == null)
                    session.FraisSupplementaireSessions = new List<FraisSupplementaireSession>();

                session.FraisSupplementaireSessions.Add(existing);

                _context.FraisSupplementaireSessions.Add(existing);
            }
            else
            {

                var fs = _context.FraisSupplementaires.FirstOrDefault(i => i.Id == fraisSupplementaireSession.FraisSupplementaireId);
                var session = _context.Sessions.Include(i => i.FraisSupplementaireSessions).FirstOrDefault(i => i.Id == fraisSupplementaireSession.SessionId);

                existing.SessionId = fraisSupplementaireSession.SessionId;
                existing.Session = session;
                existing.FraisSupplementaireId = fraisSupplementaireSession.FraisSupplementaireId;
                existing.FraisSupplementaire = fs;
                existing.Quantite = fraisSupplementaireSession.Quantite;
                existing.DateModification = DateTime.UtcNow;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existing;
            else
                return null;
        }

        public async Task<Facture> SaveFactureAsync(Facture facture)
        {
            var existingFacture = _context.Factures
             .Include(c => c.Entreprise)
             .Include(c => c.Sessions)
             .SingleOrDefault(c => c.Id == facture.Id);

            var newEntreprise = _context.Entreprises.SingleOrDefault(c => c.Id == facture.Entreprise.Id);
            if (newEntreprise == null)
                return null;
            var newSessions = new List<Session>();
            foreach(var s in facture.Sessions)
            {
                newSessions.Add(_context.Sessions.SingleOrDefault(c => c.Id == s.Id));
            }


            if (existingFacture == null)
            {
                existingFacture = new Facture
                {
                    Entreprise = newEntreprise,
                    Sessions = newSessions,
                    BonCommande = facture.BonCommande,
                    Date = facture.Date,
                    PeriodeDebut = facture.PeriodeDebut,
                    PeriodeFin = facture.PeriodeFin,
                    DateCreation = DateTimeOffset.UtcNow,
                    DateModification = DateTimeOffset.UtcNow
                };

                _context.Factures.Add(existingFacture);
            }
            else
            {
                var ids = facture.Sessions.Select(ss => ss.Id).ToList();

                foreach (var ss in existingFacture.Sessions.ToList())
                {
                    // Remove the roles which are not in the list of new roles
                    if (!ids.Contains(ss.Id))
                        existingFacture.Sessions.Remove(ss);
                    // Removes role 3 in the example
                }

                foreach (var id in ids)
                {
                    // Add the roles which are not in the list of user's roles
                    if (!existingFacture.Sessions.Any(r => r.Id == id))
                    {
                        var session = new Session { Id = id };
                        _context.Sessions.Attach(session);
                        existingFacture.Sessions.Add(session);
                    }
                    // Adds roles 1 and 2 in the example
                }

                _context.Entry(existingFacture).State = EntityState.Modified;
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
                return existingFacture;
            else
                return null;
        }

        /*public async Task<bool> SaveSessionAsync(Session newSession)
        {
            var existingSession = _context.Sessions
                 .Include(c => c.Formation)
                 .Include(c => c.Formateur)
                 .Include(c => c.Salle)
                 .Include(c => c.PersonneSessions)
                 .ThenInclude(c => c.Personne)
                 .Include(c => c.ParticipantSessions)
                 .Single(c => c.Id == newSession.Id);

            if (existingSession == null) return false;

            int changes = 0;
            #region PersonneSession
            var personneSessions = new List<PersonneSession>();
            if (newSession.PersonneIds != null)
            {
                foreach (var personneId in newSession.PersonneIds)
                {
                    var personneSession = GetPersonneSessionForId(personneId, existingSession.Id);
                    if (personneSession == null)
                    {
                        personneSession = new PersonneSession
                        {
                            PersonneId = personneId,
                            Personne = GetPersonneForId(personneId),
                            SessionId = existingSession.Id,
                            Session = existingSession
                        };
                    }
                    personneSessions.Add(personneSession);
                }
            }

            if (existingSession.PersonneSessions == null)
            {
                existingSession.PersonneSessions = new List<PersonneSession>();
            }

            foreach (var newPersonneSession in personneSessions)
            {
                if (!existingSession.PersonneSessions.Contains(newPersonneSession))
                {
                    existingSession.PersonneSessions.Add(newPersonneSession);
                    changes++;
                }
            }

            var personneSessionsClonelist = new List<PersonneSession>(existingSession.PersonneSessions);

            foreach (var personneSession in personneSessionsClonelist)
            {
                if (!personneSessions.Contains(personneSession))
                {
                    existingSession.PersonneSessions.Remove(personneSession);
                    changes++;
                }
            }
            #endregion

            #region FraisSupplementaireSession
            var fraisSupplementaireSessions = new List<FraisSupplementaireSession>();
            if (newSession.NewFraisSupplementaires != null)
            {
                foreach (var fs in newSession.NewFraisSupplementaires)
                {
                    var fraisSupplementaireSession = GetFraisSupplementaireSessionForId(fs.FraisSupplementaireId, existingSession.Id);
                    if (fraisSupplementaireSession == null)
                    {
                        fraisSupplementaireSession = new FraisSupplementaireSession
                        {
                            FraisSupplementaireId = fs.FraisSupplementaireId,
                            FraisSupplementaire = GetFraisSupplementaireForId(fs.FraisSupplementaireId),
                            SessionId = existingSession.Id,
                            Session = existingSession,
                            Quantite = fs.Quantite
                        };
                    }
                    else
                    {
                        fraisSupplementaireSession.Quantite = fs.Quantite;
                    }
                    fraisSupplementaireSessions.Add(fraisSupplementaireSession);
                }
            }

            if (existingSession.FraisSupplementaireSessions == null)
            {
                existingSession.FraisSupplementaireSessions = new List<FraisSupplementaireSession>();
            }

            foreach (var newfraisSupplementaireSession in fraisSupplementaireSessions)
            {
                if (!existingSession.FraisSupplementaireSessions.Contains(newfraisSupplementaireSession))
                {
                    existingSession.FraisSupplementaireSessions.Add(newfraisSupplementaireSession);
                    changes++;
                }
            }

            var fraisSupplementaireSessionsClonelist = new List<FraisSupplementaireSession>(existingSession.FraisSupplementaireSessions);

            foreach (var fraisSupplementaireSession in fraisSupplementaireSessionsClonelist)
            {
                if (!fraisSupplementaireSessions.Contains(fraisSupplementaireSession))
                {
                    existingSession.FraisSupplementaireSessions.Remove(fraisSupplementaireSession);
                    changes++;
                }
            }

            #endregion

            #region ParticipantSession
            var participantSessions = new List<ParticipantSession>();
            if (newSession.NewParticipants != null)
            {
                foreach (var newParticipant in newSession.NewParticipants)
                {

                    var participantSession = GetParticipantSessionForId(newParticipant.Id, existingSession.Id);
                    if (participantSession == null)
                    {
                        newParticipant.Id = Guid.NewGuid();
                        Accessoire accessoire = null;
                        Guid accessoireId = Guid.Empty;
                        if (Guid.TryParse(newParticipant.Accessoire.Val, out accessoireId))
                        {
                            accessoire = GetAccessoireForId(accessoireId);
                        }
                        Personne personne = null;
                        Guid personneId = Guid.Empty;
                        if (Guid.TryParse(newParticipant.Personne.Val, out personneId))
                        {
                            personne = GetPersonneForId(personneId);
                        }

                        participantSession = new ParticipantSession
                        {
                            ParticipantId = newParticipant.Id,
                            Participant = new Participant
                            {
                                Id = newParticipant.Id,
                                Absence = newParticipant.Absence,
                                Accessoire = accessoire,
                                AccessoireSeulement = newParticipant.AccessoireSeulement,
                                Personne = personne,
                                Grandeur = newParticipant.Grandeur,
                                DateModification = DateTimeOffset.Now,
                                DateCreation = DateTimeOffset.Now
                            },
                            SessionId = existingSession.Id,
                            Session = existingSession
                        };
                        changes++;
                    }
                    else
                    {
                        Accessoire accessoire = null;
                        Guid accessoireId = Guid.Empty;
                        if (newParticipant.Accessoire != null)
                        {
                            if (Guid.TryParse(newParticipant.Accessoire.Val, out accessoireId))
                            {
                                accessoire = GetAccessoireForId(accessoireId);
                            }
                        }

                        Personne personne = null;
                        Guid personneId = Guid.Empty;
                        if (newParticipant.Personne != null)
                        {
                            if (Guid.TryParse(newParticipant.Personne.Val, out personneId))
                            {
                                personne = GetPersonneForId(personneId);
                            }
                        }

                        participantSession.Participant.Absence = newParticipant.Absence;
                        participantSession.Participant.AccessoireSeulement = newParticipant.AccessoireSeulement;
                        participantSession.Participant.Grandeur = newParticipant.Grandeur;
                        participantSession.Participant.Accessoire = accessoire;
                        participantSession.Participant.Personne = personne;
                        participantSession.Participant.DateModification = DateTimeOffset.UtcNow;
                        changes++;
                    }
                    participantSessions.Add(participantSession);
                }
            }

            if (existingSession.ParticipantSessions == null)
            {
                existingSession.ParticipantSessions = new List<ParticipantSession>();
            }

            foreach (var newParticipantSession in participantSessions)
            {
                if (!existingSession.ParticipantSessions.Contains(newParticipantSession))
                {
                    existingSession.ParticipantSessions.Add(newParticipantSession);
                    changes++;
                }
            }

            var participantSessionsClonelist = new List<ParticipantSession>(existingSession.ParticipantSessions);

            foreach (var participantSession in participantSessionsClonelist)
            {
                if (!participantSessions.Contains(participantSession))
                {
                    existingSession.ParticipantSessions.Remove(participantSession);
                    changes++;
                }
            }
            #endregion

            var newFormation = _context.Formations.Single(c => c.Id == newSession.Formation);
            var newFormateur = _context.Formateurs.Single(c => c.Id == newSession.Formateur);
            var newEntreprise = _context.Entreprises.Single(c => c.Id == newSession.Entreprise);
            var newContact = _context.Contacts.Single(c => c.Id == newSession.Contact);
            var newSalle = _context.Salles.Single(c => c.Id == newSession.Salle);

            existingSession.EntrepriseId = newSession.Entreprise;
            existingSession.Entreprise = newEntreprise;
            existingSession.ContactId = newSession.Contact;
            existingSession.Contact = newContact;
            existingSession.Formation = newFormation;
            existingSession.Formateur = newFormateur;
            existingSession.Salle = newSalle;
            existingSession.BonCommande = newSession.BonCommande;
            existingSession.Date = newSession.Date;
            existingSession.UtilisePrixSession = newSession.UtilisePrixSession;
            existingSession.Prix = newSession.Prix;
            existingSession.UtiliseDureeSession = newSession.UtiliseDureeSession;
            existingSession.Duree = newSession.Duree;
            existingSession.TypeFacturation = newSession.TypeFacturation;
            existingSession.NombrePersonnesAttendues = newSession.NombrePersonnesAttendues;
            existingSession.DateModification = DateTimeOffset.UtcNow;

            _context.Entry(existingSession).State = EntityState.Modified;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == (changes + 1);
        }

        public async Task<bool> SaveFactureAsync(NewFacture newFacture)
        {
            int changes = 0;
            var sessions = new List<Session>();
            if (newFacture.SessionIds != null)
            {
                foreach (var id in newFacture.SessionIds)
                {
                    sessions.Add(GetSessionForId(id));
                }
            }

            var existingFacture = _context.Factures
             .Include(c => c.Entreprise)
             .Include(c => c.Sessions)
             .Single(c => c.Id == newFacture.Id);

            if (existingFacture == null) return false;

            if (existingFacture.Sessions == null)
            {
                existingFacture.Sessions = new List<Session>();
            }

            foreach (var newSession in sessions)
            {
                if (!existingFacture.Sessions.Contains(newSession))
                {
                    existingFacture.Sessions.Add(newSession);
                    changes++;
                }
            }

            var sessionsClonelist = new List<Session>(existingFacture.Sessions);

            foreach (var session in sessionsClonelist)
            {
                if (!sessions.Contains(session))
                {
                    existingFacture.Sessions.Remove(session);
                    changes++;
                }
            }

            var newEntreprise = _context.Entreprises.Single(c => c.Id == newFacture.EntrepriseId);

            existingFacture.Entreprise = newEntreprise;
            existingFacture.BonCommande = newFacture.BonCommande;
            existingFacture.PeriodeDebut = newFacture.PeriodeDebut;
            existingFacture.PeriodeFin = newFacture.PeriodeFin;
            existingFacture.DateModification = DateTimeOffset.UtcNow;
            existingFacture.Date = newFacture.Date;

            _context.Entry(existingFacture).State = EntityState.Modified;
            var saveResult = await _context.SaveChangesAsync();

            return saveResult == (changes + 1);
        }

        /*public async Task<bool> SaveSessionFromEventAsync(Event ev)
       {
           var existingSession = _context.Sessions
                .Include(c => c.Formation)
                .Include(c => c.Formateur)
                .Include(c => c.Salle)
                .Include(c => c.PersonneSessions)
                .ThenInclude(c => c.Personne)
                .Single(c => c.Id == ev.Id);

           if (existingSession == null) return false;

           int changes = 0;
           var personneSessions = new List<PersonneSession>();
           if (newSession.PersonneIds != null)
           {
               foreach (var personneId in newSession.PersonneIds)
               {
                   var personneSession = GetPersonneSessionForId(personneId, existingSession.Id);
                   if (personneSession == null)
                   {
                       personneSession = new PersonneSession
                       {
                           PersonneId = personneId,
                           Personne = GetPersonneForId(personneId),
                           SessionId = existingSession.Id,
                           Session = existingSession
                       };
                   }
                   personneSessions.Add(personneSession);
               }
           }

           if (existingSession.PersonneSessions == null)
           {
               existingSession.PersonneSessions = new List<PersonneSession>();
           }

           foreach (var newPersonneSession in personneSessions)
           {
               if (!existingSession.PersonneSessions.Contains(newPersonneSession))
               {
                   existingSession.PersonneSessions.Add(newPersonneSession);
                   changes++;
               }
           }

           var personneSessionsClonelist = new List<PersonneSession>(existingSession.PersonneSessions);

           foreach (var personneSession in personneSessionsClonelist)
           {
               if (!personneSessions.Contains(personneSession))
               {
                   existingSession.PersonneSessions.Remove(personneSession);
                   changes++;
               }
           }

           var newFormation = _context.Formations.Single(c => c.Id == newSession.Formation);
           var newFormateur = _context.Formateurs.Single(c => c.Id == newSession.Formateur);
           var newSalle = _context.Salles.Single(c => c.Id == newSession.Salle);

           existingSession.Formation = newFormation;
           existingSession.Formateur = newFormateur;
           existingSession.Salle = newSalle;
           existingSession.Date = newSession.Date;
           existingSession.DateModification = DateTimeOffset.UtcNow;

           _context.Entry(existingSession).State = EntityState.Modified;
           var saveResult = await _context.SaveChangesAsync();
           return saveResult == (changes + 1);
       }*/

        #endregion

        #region DeleteAsync
        public async Task<bool> DeleteSessionAsync(Guid id)
        {
            var session = new Session { Id = id };
            _context.Entry(session).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> DeleteFactureAsync(Guid id)
        {
            int changes = 0;
            var facture = _context.Factures
             .Include(c => c.Sessions)
             .Single(c => c.Id == id);

            if (facture.Sessions != null)
            {
                var sessionsClonelist = new List<Session>(facture.Sessions);
                foreach (var session in sessionsClonelist)
                {
                    facture.Sessions.Remove(session);
                    changes++;
                }
            }
            _context.Entry(facture).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == (changes + 1);
        }

        public async Task<bool> DeleteEntrepriseAsync(Guid id)
        {
            var entreprise = new Entreprise { Id = id };
            _context.Entry(entreprise).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteFormateurAsync(Guid id)
        {
            var session = new Formateur { Id = id };
            _context.Entry(session).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteTypeAccessoireAsync(Guid id)
        {
            var typeAccessoire = new TypeAccessoire { Id = id };
            _context.Entry(typeAccessoire).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteAccessoireAsync(Guid id)
        {
            var accessoire = new Accessoire { Id = id };
            _context.Entry(accessoire).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeletePersonneAsync(Guid id)
        {
            var personne = new Personne { Id = id };
            _context.Entry(personne).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteFormationAsync(Guid id)
        {
            var formation = new Formation { Id = id };
            _context.Entry(formation).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteContactAsync(Guid id)
        {
            var contact = new Contact { Id = id };
            _context.Entry(contact).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteFraisSupplementaireAsync(Guid id)
        {
            var frais = new FraisSupplementaire { Id = id };
            _context.Entry(frais).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteSalleAsync(Guid id)
        {
            var salle = new Salle { Id = id };
            _context.Entry(salle).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteParticipantAsync(Guid id)
        {
            var participant = new Participant { Id = id };
            _context.Entry(participant).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteFraisSupplementaireSessionAsync(Guid sessionId, Guid fraisSupplementaireId)
        {
            var fraisSupplementaireSession = new FraisSupplementaireSession { SessionId = sessionId, FraisSupplementaireId = fraisSupplementaireId };
            _context.Entry(fraisSupplementaireSession).State = EntityState.Deleted;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }
        #endregion
    }
}
