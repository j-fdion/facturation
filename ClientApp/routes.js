import Auth from '@okta/okta-vue'

import Personne from 'components/personne/personne'
import Entreprise from 'components/entreprise/entreprise'
import Formateur from 'components/formateur/formateur'
import TypeAccessoire from 'components/typeAccessoire/typeAccessoire'
import Accessoire from 'components/accessoire/accessoire'
import Formation from 'components/formation/formation'
import Contact from 'components/contact/contact'
import FraisSupplementaire from 'components/fraisSupplementaire/fraisSupplementaire'
import Salle from 'components/salle/salle'
import Session from 'components/session/session'
import Facture from 'components/facture/facture'
import HomePage from 'components/home-page'
import Calendrier from 'components/calendrier/calendrier'


export const routes = [
    { path: '/', component: HomePage, display: 'Accueil', style: 'glyphicon glyphicon-home', draw: true},
    { path: '/calendrier', component: Calendrier, display: 'Calendrier', style: 'glyphicon glyphicon-home', draw: true},
    { path: '/personne', component: Personne, display: 'Personnes', style: 'glyphicon glyphicon-th-list', draw: true},
    { path: '/entreprise', component: Entreprise, display: 'Entreprises', style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/formateur', component: Formateur, display: 'Formateurs', style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/typeAccessoire', component: TypeAccessoire, display: "Type d'accessoires", style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/accessoire', component: Accessoire, display: "Accessoires", style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/formation', component: Formation, display: "Formations", style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/contact', component: Contact, display: "Contacts", style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/fraisSupplementaire', component: FraisSupplementaire, display: "Frais supplémentaires", style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/salle', component: Salle, display: "Salles", style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/session', component: Session, display: "Sessions", style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/facture', component: Facture, display: "Factures", style: 'glyphicon glyphicon-th-list', draw: true },
    { path: '/implicit/callback', component: Auth.handleCallback() },
]
