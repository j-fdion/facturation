<template>
    <div>
        <session-form>
        </session-form>
        <v-card>
            <v-card-title>
                <v-btn color="primary" dark @click.native.stop="defaultEditItem">Nouvelle session</v-btn>
                <v-spacer></v-spacer>
                <v-text-field append-icon="search"
                              label="Search"
                              single-line
                              hide-details
                              v-model="computedSearch"
                              @keyup.enter="searchItem"></v-text-field>
            </v-card-title>
            <v-data-table :headers="headers"
                          :items="data"
                          :pagination.sync="computedPagination"
                          :total-items="totalItems"
                          :loading="loading"
                          :search="computedSearch"
                          class="elevation-1">
                <v-progress-linear slot="progress" color="blue" indeterminate></v-progress-linear>
                <template slot="items" slot-scope="props">
                    <td class="text-xs-center">{{ props.item.entreprise.nom }}</td>
                    <td class="text-xs-center">{{ props.item.formation.titre }}</td>
                    <td class="text-xs-center">{{ props.item.formateur.nomComplet }}</td>
                    <td class="text-xs-center">{{ props.item.salle.nom }}</td>
                    <td class="text-xs-center">{{ props.item.date | formatDateLLL }}</td>
                    <td class="text-xs-center">{{ props.item.bonCommande }}</td>
                    <td class="text-xs-center">{{ props.item.dateModification | formatDateLLL }}</td>
                    <td class="text-xs-center">{{ props.item.dateCreation | formatDateLLL }}</td>
                    <td class="justify-center layout px-0">
                        <v-btn icon class="mx-0" @click.native.stop="openInvoice(props.item)">
                            <v-icon color="blue">receipt</v-icon>
                        </v-btn>
                        <v-btn icon class="mx-0" @click.native.stop="editItem(props.item)">
                            <v-icon color="teal">edit</v-icon>
                        </v-btn>
                        <v-btn icon class="mx-0" @click.native.stop="deleteItem(props.item)">
                            <v-icon color="pink">delete</v-icon>
                        </v-btn>
                    </td>
                </template>
                <v-alert v-if="!loading" slot="no-data" :value="true" color="error" icon="warning">
                    Aucuns résultats disponibles
                </v-alert>
            </v-data-table>
        </v-card>
        <div v-html="html"></div>
    </div>
</template>

<script>

    import { createNamespacedHelpers } from 'vuex'
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('session')

    import axios from 'axios'
    import * as module from '../../store/modules/session/session'

    export default {
        data: () => ({
            headers: [
                { text: 'Entreprise', align: 'center', sortable: true, value: 'Entreprise.Nom' },
                { text: 'Formation', align: 'center', sortable: true, value: 'Formation.Titre' },
                { text: 'Formateur', align: 'center', sortable: true, value: 'Formateur.Prenom' },
                { text: 'Salle', align: 'center', sortable: true, value: 'Salle.Nom' },
                { text: 'Date', align: 'center', value: 'Date' },
                { text: 'Bon de commande', align: 'center', value: 'BonCommande' },
                { text: 'Modifié', align: 'center', value: 'DateModification' },
                { text: 'Créé', align: 'center', value: 'DateCreation' },
                { text: 'Actions', align: 'center', value: 'Actions', sortable: false }
            ],
            search: '',
            pagination: {
                search: '',
                sortBy: "DateCreation",
                descending: true,
            },
            defaultPagination: {
                sortBy: "DateCreation",
                descending: true,
            },
            html: '<p>Loading...</p>'
        }),
        computed: {
            data: {
                get() {
                    return this.items;
                }

            },
            computedSearch: {
                get() {
                    return this.search;
                },
                set(value) {
                    this.search = value;
                    this.pagination.search = this.search;
                    if (this.pagination.sortBy == null || this.pagination.descending == null)
                        this.pagination = Object.assign({}, this.pagination, this.defaultPagination);
                }
            },
            computedPagination: {
                get() {
                    return this.pagination;
                },
                set(value) {
                    this.pagination = value;
                    this.pagination.search = this.search;
                    if (this.pagination.sortBy == null || this.pagination.descending == null)
                        this.pagination = Object.assign({}, this.pagination, this.defaultPagination);
                    this.LOAD_ITEMS({ payload: this.pagination });
                }
            },
            ...mapState([module.items, module.loading]), 
            ...mapGetters([module.items, module.totalItems, module.editedItem])
        },

        mounted: function () {
            if (this.pagination.sortBy != null && this.pagination.descending != null)
                this.LOAD_ITEMS({ payload: this.pagination });
            else
                this.LOAD_ITEMS({ payload: this.defaultPagination });
        },

        methods: {
            ...mapActions([module.LOAD_ITEMS, module.OPEN_DEFAULT_DIALOG, module.OPEN_DIALOG, module.OPEN_DELETE_DIALOG]),
            defaultEditItem(item, event) {
                this.OPEN_DEFAULT_DIALOG();
            },
            editItem(item, event) {
                this.OPEN_DIALOG({ payload: item });
            },
            deleteItem(item, event) {
                this.OPEN_DELETE_DIALOG({ payload: item });
            },
            searchItem(event) {
                this.LOAD_ITEMS({ payload: this.pagination });
            },
            openInvoice(item, event) {
                //'/api/Gestion/SearchSessions'
                self = this
                axios.get('/api/Gestion/FeuillePresence', {
                    params: {
                        'id': item.id
                    }
                }).then(function (response) {
                    console.log(response)
                    self.html = response.data;
                }, (err) => {
                    console.log(err)
                })
                //window.open('/api/Gestion/FeuillePresence?id=' + item.id, '_blank');
            },
        }
}
</script>
