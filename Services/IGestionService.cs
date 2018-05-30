using AppFactu.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vueAppFactu.Models;

namespace AppFactu.Services
{
    public interface IGestionService
    {
        #region GetAll
        IQueryable<Personne> GetPersonnes();

        IQueryable<Personne> GetPersonnesForEntreprise(Guid entrepriseId);

        IQueryable<Formation> GetFormations();

        IQueryable<Formateur> GetFormateurs();

        IQueryable<Formateur> GetPagedFormateurs(int skip, int take);

        IQueryable<Session> GetSessions();

        IQueryable<Session> GetSessionsBetweenDates(DateTimeOffset start, DateTimeOffset end);

        IQueryable<Entreprise> GetEntreprises();

        IQueryable<Contact> GetContacts();

        IQueryable<PersonneSession> GetPersonneSessions();

        IQueryable<Salle> GetSalles();

        IQueryable<FraisSupplementaire> GetFraisSupplementaires();

        IQueryable<Facture> GetFactures();

        IQueryable<Accessoire> GetAccessoires();

        IQueryable<TypeAccessoire> GetTypeAccessoires();
        #endregion

        #region GetForId
        Personne GetPersonneForId(Guid personneId);

        Formation GetFormationForId(Guid formationId);

        Formateur GetFormateurForId(Guid formateurId);

        Session GetSessionForId(Guid sessionId);

        List<Session> GetSessionsForPersonneId(Guid personneId);

        Entreprise GetEntrepriseForId(Guid entrepriseId);

        Contact GetContactForId(Guid contactId);

        ContactEntreprise GetContactEntrepriseForId(Guid contactId, Guid entrepriseId);

        PersonneSession GetPersonneSessionForId(Guid personneId, Guid sessionId);

        List<PersonneSession> GetPersonneSessionForId(Guid sessionId);

        FormateurFormation GetFormateurFormationForId(Guid formateurId, Guid formationId);

        List<FormateurFormation> GetFormateurFormationForId(Guid formateurId);

        Salle GetSalleForId(Guid salleId);

        FraisSupplementaire GetFraisSupplementaireForId(Guid fraisId);

        Facture GetFactureForId(Guid factureId);

        Accessoire GetAccessoireForId(Guid id);

        TypeAccessoire GetTypeAccessoireForId(Guid id);

        Participant GetParticipantForId(Guid id);

        ParticipantSession GetParticipantSessionForId(Guid participantId, Guid sessionId);

        List<ParticipantSession> GetParticipantSessionForId(Guid sessionId);

        ParticipantSession GetParticipantSessionForPersonneIdAndSessionId(Guid personneId, Guid sessionId);

        IQueryable<Participant> GetParticipantsForSessionId(Guid sessionId);

        IQueryable<FraisSupplementaireSession> GetFraisSupplementaireSessionsForSessionId(Guid sessionId);
        #endregion

        Task<Entreprise> SaveEntrepriseAsync(Entreprise entreprise);

        Task<TypeAccessoire> SaveTypeAccessoireAsync(TypeAccessoire typeAccessoire);

        Task<Personne> SavePersonneAsync(Personne personne);

        Task<Formateur> SaveFormateurAsync(Formateur formateur);

        Task<Accessoire> SaveAccessoireAsync(Accessoire accessoire);

        Task<Formation> SaveFormationAsync(Formation formation);

        Task<Contact> SaveContactAsync(Contact contact);

        Task<FraisSupplementaire> SaveFraisSupplementaireAsync(FraisSupplementaire newFrais);

        Task<Salle> SaveSalleAsync(Salle salle);

        Task<Session> SaveSessionAsync(Session session);

        Task<Participant> SaveParticipantAsync(Participant participant);

        Task<FraisSupplementaireSession> SaveFraisSupplementaireSessionAsync(FraisSupplementaireSession fraisSupplementaireSession);

        Task<Facture> SaveFactureAsync(Facture facture);

        /// 

        //Task<bool> AddFactureAsync(Facture newFacture);
        //Task<bool> SaveFactureAsync(Facture newFacture);



        Task<bool> DeleteSessionAsync(Guid id);
        Task<bool> DeleteFactureAsync(Guid id);

        ///

        Task<bool> DeletePersonneAsync(Guid id);

        Task<bool> DeleteEntrepriseAsync(Guid id);

        Task<bool> DeleteFormateurAsync(Guid id);

        Task<bool> DeleteTypeAccessoireAsync(Guid id);

        Task<bool> DeleteAccessoireAsync(Guid id);

        Task<bool> DeleteFormationAsync(Guid id);

        Task<bool> DeleteContactAsync(Guid id);

        Task<bool> DeleteFraisSupplementaireAsync(Guid id);

        Task<bool> DeleteSalleAsync(Guid id);

        Task<bool> DeleteParticipantAsync(Guid id);

        Task<bool> DeleteFraisSupplementaireSessionAsync(Guid sessionId, Guid fraisSupplementaireId);
    }
}
