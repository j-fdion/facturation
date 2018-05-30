import axios from 'axios'
import Vue from 'vue'
import moment from 'moment'

//Default datatable actions/mutations
export const SAVE_ITEM = 'SAVE_ITEM'
export const LOAD_ITEMS = 'LOAD_ITEMS'
export const DELETE_ITEM = 'DELETE_ITEM'
export const REMOVE_ITEM = 'REMOVE_ITEM'
export const SET_ITEMS = 'SET_ITEMS'
export const SET_TOTAL_ITEMS = 'SET_TOTAL_ITEMS'
export const SET_ITEM = 'SET_ITEM'
export const SET_LOADING_STATUS = 'SET_LOADING_STATUS'

//Dialog
export const SET_DIALOG = 'SET_DIALOG'
export const SET_DELETE_DIALOG = 'SET_DELETE_DIALOG'
export const OPEN_DIALOG = 'OPEN_DIALOG'
export const OPEN_DEFAULT_DIALOG = 'OPEN_DEFAULT_DIALOG'
export const OPEN_DELETE_DIALOG = 'OPEN_DELETE_DIALOG'
export const CLOSE_DIALOG = 'CLOSE_DIALOG'
export const SET_EDITED_ITEM = 'SET_EDITED_ITEM'
export const SET_EDITED_ITEM_DEFAULT = 'SET_EDITED_ITEM_DEFAULT'

//Default state
export const items = 'items'
export const totalItems = 'totalItems'
export const dialog = 'dialog'
export const editedIndex = 'editedIndex'
export const editedItem = 'editedItem'
export const loading = 'loading'

//Module specific constants
const load_items_url = '/api/Gestion/Sessions'
const save_item_url = '/api/Gestion/SaveSession'
const delete_item_url = '/api/Gestion/DeleteSession'
export const MERGE_EDIT_TO_ITEM = 'MERGE_EDIT_TO_ITEM'

export const SEARCH_ENTREPRISES = 'SEARCH_ENTREPRISES'
export const SET_ENTREPRISES = 'SET_ENTREPRISES'
export const searchedEntreprises = 'searchedEntreprises'

export const SEARCH_CONTACTS = 'SEARCH_CONTACTS'
export const SET_CONTACTS = 'SET_CONTACTS'
export const searchedContacts = 'searchedContacts'

export const SEARCH_FORMATIONS = 'SEARCH_FORMATIONS'
export const SET_FORMATIONS = 'SET_FORMATIONS'
export const searchedFormations = 'searchedFormations'

export const SEARCH_FORMATEURS = 'SEARCH_FORMATEURS'
export const SET_FORMATEURS = 'SET_FORMATEURS'
export const searchedFormateurs = 'searchedFormateurs'

export const SEARCH_SALLES = 'SEARCH_SALLES'
export const SET_SALLES = 'SET_SALLES'
export const searchedSalles = 'searchedSalles'

export const SET_EDITED_ITEM_WITH_DATE = 'SET_EDITED_ITEM_WITH_DATE'
export const OPEN_DIALOG_WITH_DATE = 'OPEN_DIALOG_WITH_DATE'

const state = {
    items: [],
    loading: true,
    searchedEntreprises: [],
    searchedContacts: [],
    searchedFormations: [],
    searchedFormateurs: [],
    searchedSalles: [],
    totalItems: 0,
    dialog: false,
    deleteDialog: false,
    editedIndex: -1,
    editedItem: {
        id: '00000000-0000-0000-0000-000000000000',
        entreprise: {
            id: '00000000-0000-0000-0000-000000000000',
            nom: ''
        },
        contact: {
            id: '00000000-0000-0000-0000-000000000000',
            nomComplet: ''
        },
        formation: {
            id: '00000000-0000-0000-0000-000000000000',
            titre: ''
        },
        formateur: {
            id: '00000000-0000-0000-0000-000000000000',
            nomComplet: ''
        },
        salle: {
            id: '00000000-0000-0000-0000-000000000000',
            nom: ''
        },
        dateClient: null,
        timeClient: null,
        date: null,
        bonCommande: '',
        typeFacturation: 'Horaire',
        utiliseDureeSession: false,
        duree: 0.0,
        utilisePrixSession: false,
        prix: 0.0,
        nombrePersonnesAttendues: 0,
        dateCreation: '',
        dateModification: '',
    },
    defaultItem: {
        id: '00000000-0000-0000-0000-000000000000',
        entreprise: {
            id: '00000000-0000-0000-0000-000000000000',
            nom: ''
        },
        contact: {
            id: '00000000-0000-0000-0000-000000000000',
            nomComplet: ''
        },
        formation: {
            id: '00000000-0000-0000-0000-000000000000',
            titre: ''
        },
        formateur: {
            id: '00000000-0000-0000-0000-000000000000',
            nomComplet: ''
        },
        salle: {
            id: '00000000-0000-0000-0000-000000000000',
            nom: ''
        },
        dateClient: null,
        timeClient: null,
        date: null,
        bonCommande: '',
        typeFacturation: 'Horaire',
        utiliseDureeSession: false,
        duree: 0.0,
        utilisePrixSession: false,
        prix: 0.0,
        nombrePersonnesAttendues: 0,
        dateCreation: '',
        dateModification: '',
    }
}

const getters = ({
    items: state => {
        return state.items
    },
    totalItems: state => {
        return state.totalItems
    },
    editedItem: state => {
        return state.editedItem
    }
})

const mutations = {

    SET_ITEMS: (state, { payload }) => {
        state.items = payload
    },
    SET_TOTAL_ITEMS: (state, { payload }) => {
        state.totalItems = payload
    },
    SET_LOADING_STATUS: (state, { payload }) => {
        state.loading = payload
    },
    SET_ITEM: (state, {payload}) => {
        if (state.editedIndex == -1) {
            state.items.push(payload)
        }
        else {
            Vue.set(state.items, state.editedIndex, payload)
        }
    },
    REMOVE_ITEM: (state) => {
        if (state.editedIndex != -1)
            Vue.delete(state.items, state.editedIndex);
    },
    SET_DIALOG: (state, { payload }) => {
        state.dialog = payload
    },
    SET_DELETE_DIALOG: (state, { payload }) => {
        state.deleteDialog = payload
    },
    SET_EDITED_ITEM: (state, { payload }) => {
        state.editedIndex = state.items.indexOf(payload);
        state.editedItem = Object.assign({}, state.editedItem, payload);
        state.editedItem.dateClient = moment.utc(state.editedItem.date).locale("fr-ca").format("L");
        state.editedItem.timeClient = moment.utc(state.editedItem.date).locale("fr-ca").format("LT");
        state.searchedEntreprises = [state.editedItem.entreprise]
        state.searchedContacts = [state.editedItem.contact]
        state.searchedFormations = [state.editedItem.formation]
        state.searchedFormateurs = [state.editedItem.formateur]
        state.searchedSalles = [state.editedItem.salle]
    },
    SET_EDITED_ITEM_DEFAULT: (state) => {
        state.editedIndex = -1
        state.editedItem = Object.assign({}, state.defaultItem);
        state.searchedEntreprises = []
        state.searchedContacts = []
        state.searchedFormations = []
        state.searchedFormateurs = []
        state.searchedSalles = []
    },
    SET_EDITED_ITEM_WITH_DATE: (state, { payload }) => {
        state.editedIndex = -1
        state.editedItem = Object.assign({}, state.defaultItem);
        state.editedItem.date = payload
        state.editedItem.dateClient = moment.utc(state.editedItem.date).locale("fr-ca").format("L");
        state.editedItem.timeClient = moment.utc(state.editedItem.date).locale("fr-ca").format("LT");
        state.searchedEntreprises = []
        state.searchedContacts = []
        state.searchedFormations = []
        state.searchedFormateurs = []
        state.searchedSalles = []
    },

    SET_ENTREPRISES: (state, { payload }) => {
        state.searchedEntreprises = payload
    },
    SET_CONTACTS: (state, { payload }) => {
        state.searchedContacts = payload
    },
    SET_FORMATIONS: (state, { payload }) => {
        state.searchedFormations = payload
    },
    SET_FORMATEURS: (state, { payload }) => {
        state.searchedFormateurs = payload
    },
    SET_SALLES: (state, { payload }) => {
        state.searchedSalles = payload
    },
    MERGE_EDIT_TO_ITEM: (state, { payload }) => {
        state.editedItem = Object.assign({}, state.editedItem, payload);
    }
}

const actions = ({
    LOAD_ITEMS: function ({ commit }, { payload }) {
        commit(SET_LOADING_STATUS, { payload: true });
        axios.post(load_items_url, JSON.parse(JSON.stringify(payload)))
            .then((response) => {
                commit(SET_ITEMS, { payload: response.data.rows });
                commit(SET_TOTAL_ITEMS, { payload: response.data.totalRows });
                commit(SET_LOADING_STATUS, { payload: false });
        }, (err) => {
            console.log(err)
        })
    },
    SAVE_ITEM: function ({ commit, dispatch }) {
        axios.post(save_item_url, JSON.parse(JSON.stringify(state.editedItem)))
            .then(function (response) {
                commit(SET_ITEM, { payload: response.data })
                dispatch(CLOSE_DIALOG)
            }, (err) => {
                console.log(err)
            })
    },
    DELETE_ITEM: function ({ commit, dispatch }) {
        console.log(DELETE_ITEM)
        axios.post(delete_item_url, JSON.parse(JSON.stringify(state.editedItem)))
            .then(function (response) {
                commit(REMOVE_ITEM)
                dispatch(CLOSE_DIALOG)
            }, (err) => {
                console.log(err)
            })
    },
    OPEN_DEFAULT_DIALOG: function ({ commit }) {
        commit(SET_EDITED_ITEM_DEFAULT)
        commit(SET_DIALOG, { payload: true })
    },
    OPEN_DIALOG_WITH_DATE: function ({ commit }, { payload }) {
        commit(SET_EDITED_ITEM_WITH_DATE, { payload })
        commit(SET_DIALOG, { payload: true })
    },
    OPEN_DIALOG: function ({ commit }, { payload }) {
        commit(SET_EDITED_ITEM, { payload })
        commit(SET_DIALOG, { payload: true })
    },
    OPEN_DELETE_DIALOG: function ({ commit }, { payload }) {
        commit(SET_EDITED_ITEM, { payload })
        commit(SET_DELETE_DIALOG, { payload: true })
    },
    CLOSE_DIALOG: function ({ commit }) {
        commit(SET_EDITED_ITEM_DEFAULT)
        commit(SET_DIALOG, { payload: false })
        commit(SET_DELETE_DIALOG, { payload: false })
    },
    SEARCH_ENTREPRISES: function ({ commit }, { searchTerm }) {
        axios.get('/api/Gestion/SearchEntreprises', {
            params: {
                searchTerm
            }
        }).then(function (response) {
            commit(SET_ENTREPRISES, { payload: response.data });
            console.log(response.data);
        }, (err) => {
            console.log(err)
        })
    },
    SEARCH_CONTACTS: function ({ commit }, { searchTerm, entreprise }) {
        axios.get('/api/Gestion/SearchContactsForEntreprise', {
            params: {
                searchTerm,
                entrepriseId: entreprise.id
            }
        }).then(function (response) {
            commit(SET_CONTACTS, { payload: response.data });
            console.log(response.data);
        }, (err) => {
            console.log(err)
        })
    },
    SEARCH_FORMATIONS: function ({ commit }, { searchTerm }) {
        axios.get('/api/Gestion/SearchFormations', {
            params: {
                searchTerm
            }
        }).then(function (response) {
            commit(SET_FORMATIONS, { payload: response.data });
            console.log(response.data);
        }, (err) => {
            console.log(err)
        })
    },
    SEARCH_FORMATEURS: function ({ commit }, { searchTerm, formation }) {
        axios.get('/api/Gestion/SearchFormateursForFormation', {
            params: {
                searchTerm,
                formationId: formation.id
            }
        }).then(function (response) {
            commit(SET_FORMATEURS, { payload: response.data });
            console.log(response.data);
        }, (err) => {
            console.log(err)
        })
    },
    SEARCH_SALLES: function ({ commit }, { searchTerm }) {
        axios.get('/api/Gestion/SearchSalles', {
            params: {
                searchTerm
            }
        }).then(function (response) {
            commit(SET_SALLES, { payload: response.data });
            console.log(response.data);
        }, (err) => {
            console.log(err)
        })
    },
})

const module = {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};

export default module;
