import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

import personne from './modules/personne/personne'
import entreprise from './modules/entreprise/entreprise'
import formateur from './modules/formateur/formateur'
import typeAccessoire from './modules/typeAccessoire/typeAccessoire'
import accessoire from './modules/accessoire/accessoire'
import formation from './modules/formation/formation'
import contact from './modules/contact/contact'
import fraisSupplementaire from './modules/fraisSupplementaire/fraisSupplementaire'
import salle from './modules/salle/salle'
import session from './modules/session/session'
import participant from './modules/participant/participant'
import fraisSupplementaireSession from './modules/fraisSupplementaireSession/fraisSupplementaireSession'
import facture from './modules/facture/facture'

const store = new Vuex.Store({
    modules: {
        personne,
        entreprise,
        formateur,
        typeAccessoire,
        accessoire,
        formation,
        contact,
        fraisSupplementaire,
        salle,
        session,
        participant,
        fraisSupplementaireSession,
        facture
    },
    strict: true
})

export default store
