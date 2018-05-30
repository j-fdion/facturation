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
const load_items_url = '/api/Gestion/Participants'
const save_item_url = '/api/Gestion/SaveParticipant'
const delete_item_url = '/api/Gestion/DeleteParticipant'
export const MERGE_EDIT_TO_ITEM = 'MERGE_EDIT_TO_ITEM'

export const personnes = 'personnes'
export const SEARCH_PERSONNES = 'SEARCH_PERSONNES'
export const SET_PERSONNES = 'SET_PERSONNES'
export const accessoires = 'accessoires'
export const SEARCH_ACCESSOIRES = 'SEARCH_ACCESSOIRES'
export const SET_ACCESSOIRES = 'SET_ACCESSOIRES'

const state = {
    items: [],
    loading: true,
    personnes: [],
    accessoires: [],
    totalItems: 0,
    dialog: false,
    deleteDialog: false,
    editedIndex: -1,
    editedItem: {
        id: '00000000-0000-0000-0000-000000000000',
        grandeur: 'Petit',
        absence: false,
        accessoireSeulement: false,
        personne: {
            id: '00000000-0000-0000-0000-000000000000',
            nomComplet: ''
        },
        accessoire: {
            id: '00000000-0000-0000-0000-000000000000',
            modele: ''
        },
        sessionId: '00000000-0000-0000-0000-000000000000',
        dateCreation: '',
        dateModification: '',
    },
    defaultItem: {
        id: '00000000-0000-0000-0000-000000000000',
        grandeur: 'Petit',
        absence: false,
        accessoireSeulement: false,
        personne: {
            id: '00000000-0000-0000-0000-000000000000',
            nomComplet: ''
        },
        accessoire: {
            id: '00000000-0000-0000-0000-000000000000',
            modele: ''
        },
        sessionId: '00000000-0000-0000-0000-000000000000',
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
        console.log(state.editedIndex)
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
        state.personnes = [state.editedItem.personne]
        state.accessoires = [state.editedItem.accessoire]
    },
    SET_EDITED_ITEM_DEFAULT: (state) => {
        state.editedIndex = -1
        state.editedItem = Object.assign({}, state.defaultItem);
        state.personnes = []
        state.accessoires = []
    },
    SET_PERSONNES: (state, { payload }) => {
        state.personnes = payload
    },
    SET_ACCESSOIRES: (state, { payload }) => {
        state.accessoires = payload
    },
    MERGE_EDIT_TO_ITEM: (state, { payload }) => {
        state.editedItem = Object.assign({}, state.editedItem, payload);
    }
}

const actions = ({
    LOAD_ITEMS: function ({ commit }, { payload, sessionId }) {
        commit(SET_LOADING_STATUS, { payload: true });
        axios.post(load_items_url,JSON.parse(JSON.stringify(Object.assign({}, payload, { id: sessionId }))),
            )
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
    SEARCH_PERSONNES: function ({ commit }, { searchTerm }) {
        axios.get('/api/Gestion/SearchPersonnes', {
            params: {
                searchTerm
            }
        }).then(function (response) {
            commit(SET_PERSONNES, { payload: response.data });
        }, (err) => {
            console.log(err)
        })
    },
    SEARCH_ACCESSOIRES: function ({ commit }, { searchTerm }) {
        axios.get('/api/Gestion/SearchAccessoires', {
            params: {
                searchTerm
            }
        }).then(function (response) {
            commit(SET_ACCESSOIRES, { payload: response.data });
        }, (err) => {
            console.log(err)
        })
    }
})

const module = {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};

export default module;
