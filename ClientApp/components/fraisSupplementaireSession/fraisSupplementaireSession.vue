<template>
    <div>
        <fraisSupplementaireSession-form v-bind:id="sessionId">
        </fraisSupplementaireSession-form>
        <v-card>
            <v-card-title>
                <v-btn color="primary" dark @click.native.stop="defaultEditItem">Nouveau frais supplémentaire</v-btn>
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
                    <template v-if="props.item.fraisSupplementaire">
                        <td class="text-xs-center">{{ props.item.fraisSupplementaire.nom}}</td>
                    </template>
                    <template v-else>
                        <td class="text-xs-center"></td>
                    </template>
                    <td class="text-xs-center">{{ props.item.quantite }}</td>
                    <td class="justify-center layout px-0">
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
    </div>
</template>

<script>

    import { createNamespacedHelpers } from 'vuex'
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('fraisSupplementaireSession')

    import * as module from '../../store/modules/fraisSupplementaireSession/fraisSupplementaireSession'

    export default {
        props: ['id'],
        data: () => ({
            headers: [
                {
                    text: 'Nom',
                    align: 'center',
                    sortable: true,
                    value: 'FraisSupplementaire.Nom'
                },
                { text: 'Quantite', align: 'center', value: 'Quantite' },
                { text: 'Actions', align: 'center', value: 'Actions', sortable: false }
            ],
            search: '',
            pagination: {
                search: ''
            },
            defaultPagination: {
                sortBy: "FraisSupplementaire.Nom",
                descending: true,
            },
        }),
        computed: {
            data: {
                get() {
                    return this.items;
                }

            },
            sessionId: {
                get() {
                    return this.id;
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
                    this.LOAD_ITEMS({ payload: this.pagination, sessionId: this.sessionId });
                }
            },
            ...mapState([module.items, module.loading]), 
            ...mapGetters([module.items, module.totalItems, module.editedItem])
        },

        mounted: function () {
            if (this.pagination.sortBy != null && this.pagination.descending != null)
                this.LOAD_ITEMS({ payload: this.pagination, sessionId: this.sessionId });
            else
                this.LOAD_ITEMS({ payload: this.defaultPagination, sessionId: this.sessionId });
        },

        methods: {
            ...mapMutations([module.SET_DIALOG, module.SET_EDITED_ITEM, module.SET_ITEMS]),
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
                this.LOAD_ITEMS({ payload: this.pagination, sessionId: this.sessionId });
            }
        }
}
</script>
