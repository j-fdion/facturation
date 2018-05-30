<template>
    <v-dialog v-if="dialog" v-model="dialog" max-width="700px" fullscreen hide-overlay transition="dialog-bottom-transition">
        <v-card>
            <v-card-title>
                <span class="headline">{{ formTitle }}</span>
            </v-card-title>
            <v-card-text style="min-height: 375px;">
                <v-container grid-list-md>
                    <v-form v-model="valid" ref="form" lazy-validation>
                        <v-layout wrap>
                            <v-flex xs6>
                                <v-select label="Rechercher une entreprise"
                                          autocomplete
                                          chips
                                          required
                                          :items="searchedEntreprises"
                                          clearable
                                          item-text="nom"
                                          item-value="id"
                                          return-object
                                          :rules="singleSelectRules"
                                          :search-input.sync="searchEntreprise"
                                          v-model="entreprise">
                                    <template slot="selection" slot-scope="data">
                                        <v-chip close
                                                @input="data.parent.selectItem(data.item)"
                                                :selected="data.selected"
                                                class="chip--select-multi"
                                                :key="JSON.stringify(data.item)">
                                            {{ data.item.nom }}
                                        </v-chip>
                                    </template>
                                    <template slot="item" slot-scope="data">
                                        <template v-if="typeof data.item !== 'object'">
                                            <v-list-tile-content v-text="data.item"></v-list-tile-content>
                                        </template>
                                        <template v-else>
                                            <v-list-tile-content>
                                                <v-list-tile-title v-html="data.item.nom"></v-list-tile-title>
                                            </v-list-tile-content>
                                        </template>
                                    </template>
                                </v-select>
                            </v-flex>
                            <v-flex xs6>
                                <v-select label="Rechercher un contact"
                                          autocomplete
                                          chips
                                          required
                                          :items="searchedContacts"
                                          clearable
                                          item-text="nomComplet"
                                          item-value="id"
                                          return-object
                                          :rules="singleSelectRules"
                                          :search-input.sync="searchContact"
                                          v-model="contact">
                                    <template slot="selection" slot-scope="data">
                                        <v-chip close
                                                @input="data.parent.selectItem(data.item)"
                                                :selected="data.selected"
                                                class="chip--select-multi"
                                                :key="JSON.stringify(data.item)">
                                            {{ data.item.nomComplet }}
                                        </v-chip>
                                    </template>
                                    <template slot="item" slot-scope="data">
                                        <template v-if="typeof data.item !== 'object'">
                                            <v-list-tile-content v-text="data.item"></v-list-tile-content>
                                        </template>
                                        <template v-else>
                                            <v-list-tile-content>
                                                <v-list-tile-title v-html="data.item.nomComplet"></v-list-tile-title>
                                            </v-list-tile-content>
                                        </template>
                                    </template>
                                </v-select>
                            </v-flex>
                            <v-flex xs6>
                                <v-select label="Rechercher une formation"
                                          autocomplete
                                          chips
                                          required
                                          :items="searchedFormations"
                                          clearable
                                          item-text="titre"
                                          item-value="id"
                                          return-object
                                          :rules="singleSelectRules"
                                          :search-input.sync="searchFormation"
                                          v-model="formation">
                                    <template slot="selection" slot-scope="data">
                                        <v-chip close
                                                @input="data.parent.selectItem(data.item)"
                                                :selected="data.selected"
                                                class="chip--select-multi"
                                                :key="JSON.stringify(data.item)">
                                            {{ data.item.titre }}
                                        </v-chip>
                                    </template>
                                    <template slot="item" slot-scope="data">
                                        <template v-if="typeof data.item !== 'object'">
                                            <v-list-tile-content v-text="data.item"></v-list-tile-content>
                                        </template>
                                        <template v-else>
                                            <v-list-tile-content>
                                                <v-list-tile-title v-html="data.item.titre"></v-list-tile-title>
                                            </v-list-tile-content>
                                        </template>
                                    </template>
                                </v-select>
                            </v-flex>
                            <v-flex xs6>
                                <v-select label="Rechercher un formateur"
                                          autocomplete
                                          chips
                                          required
                                          :items="searchedFormateurs"
                                          clearable
                                          item-text="nomComplet"
                                          item-value="id"
                                          return-object
                                          :rules="singleSelectRules"
                                          :search-input.sync="searchFormateur"
                                          v-model="formateur">
                                    <template slot="selection" slot-scope="data">
                                        <v-chip close
                                                @input="data.parent.selectItem(data.item)"
                                                :selected="data.selected"
                                                class="chip--select-multi"
                                                :key="JSON.stringify(data.item)">
                                            {{ data.item.nomComplet }}
                                        </v-chip>
                                    </template>
                                    <template slot="item" slot-scope="data">
                                        <template v-if="typeof data.item !== 'object'">
                                            <v-list-tile-content v-text="data.item"></v-list-tile-content>
                                        </template>
                                        <template v-else>
                                            <v-list-tile-content>
                                                <v-list-tile-title v-html="data.item.nomComplet"></v-list-tile-title>
                                            </v-list-tile-content>
                                        </template>
                                    </template>
                                </v-select>
                            </v-flex>
                            <v-flex xs12>
                                <v-select label="Rechercher une salle"
                                          autocomplete
                                          chips
                                          required
                                          :items="searchedSalles"
                                          clearable
                                          item-text="nom"
                                          item-value="id"
                                          return-object
                                          :rules="singleSelectRules"
                                          :search-input.sync="searchSalle"
                                          v-model="salle">
                                    <template slot="selection" slot-scope="data">
                                        <v-chip close
                                                @input="data.parent.selectItem(data.item)"
                                                :selected="data.selected"
                                                class="chip--select-multi"
                                                :key="JSON.stringify(data.item)">
                                            {{ data.item.nom }}
                                        </v-chip>
                                    </template>
                                    <template slot="item" slot-scope="data">
                                        <template v-if="typeof data.item !== 'object'">
                                            <v-list-tile-content v-text="data.item"></v-list-tile-content>
                                        </template>
                                        <template v-else>
                                            <v-list-tile-content>
                                                <v-list-tile-title v-html="data.item.nom"></v-list-tile-title>
                                            </v-list-tile-content>
                                        </template>
                                    </template>
                                </v-select>
                            </v-flex>
                            <v-flex xs6>
                                <v-date-picker v-model="dateClient"
                                               required
                                               color="grey darken-4"
                                               :reactive="true"
                                               locale="fr-ca"
                                               full-width
                                               landscape>
                                </v-date-picker>
                            </v-flex>
                            <v-flex xs6>
                                <v-time-picker v-model="timeClient"
                                               required
                                               color="grey darken-4"
                                               format="24hr"
                                               full-width
                                               landscape>
                                </v-time-picker>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field label="Value" v-model="date"></v-text-field>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field label="Bon de commande" v-model="bonCommande"></v-text-field>
                            </v-flex>
                            <v-flex xs12>
                                <template v-if="!isNewEntry">
                                    <participant v-bind:id="id"></participant>
                                </template>
                                <template v-else>
                                </template>
                            </v-flex>
                            <v-flex xs12>
                                <template v-if="!isNewEntry">
                                    <fraisSupplementaireSession v-bind:id="id"></fraisSupplementaireSession>
                                </template>
                                <template v-else>
                                </template>
                            </v-flex>
                            <v-flex xs4>
                                <v-radio-group label="Type de facturation" v-model="typeFacturation" column>
                                    <v-radio label="Horaire" value="Horaire"></v-radio>
                                    <v-radio label="Forfaitaire" value="Forfaitaire"></v-radio>
                                    <v-radio label="Unitaire" value="Unitaire"></v-radio>
                                </v-radio-group>
                            </v-flex>
                            <v-flex xs12>
                                <v-layout align-center>
                                    <v-checkbox v-model="utiliseDureeSession" hide-details class="shrink mr-2"></v-checkbox>
                                    <v-text-field label="Entrer une durée (falcultatif)" v-model="duree" :rules="numberRules" :disabled="!utiliseDureeSession"></v-text-field>
                                </v-layout>
                            </v-flex>
                            <v-flex xs12>
                                <v-layout align-center>
                                    <v-checkbox v-model="utilisePrixSession" hide-details class="shrink mr-2"></v-checkbox>
                                    <v-text-field label="Entrer un prix (falcultatif)" v-model="prix" :rules="numberRules" :disabled="!utilisePrixSession"></v-text-field>
                                </v-layout>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field label="Nombre de personnes attendues" required v-model="nombrePersonnesAttendues" :rules="numberRules"></v-text-field>
                            </v-flex>
                        </v-layout>
                        </v-form>
                </v-container>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" flat @click.native="close">Annuler</v-btn>
                <v-btn color="green darken-1" flat @click.native="save" :disabled="!valid">Sauvegarder</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
    <v-dialog v-else-if="deleteDialog" v-model="deleteDialog" max-width="700px">
        <v-card>
            <v-card-title>
                <span class="headline">Supprimer un formateur</span>
            </v-card-title>
            <v-alert type="error" :value="true">
                Vous allez supprimer cet élément. Continuer?
            </v-alert>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" flat @click.native.stop="close">Annuler</v-btn>
                <v-btn color="red darken-1" flat @click.native.stop="del">Supprimer</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
    import { createNamespacedHelpers } from 'vuex'
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('session')

    import * as module from '../../store/modules/session/session'
    import moment from 'moment'

    export default {
        data: () => ({
            searchEntreprise: null,
            searchContact: null,
            searchFormation: null,
            searchFormateur: null,
            searchSalle: null,
            valid: true,
            config: {
                format: "LLL",
                locale: "fr-ca",
            },
            nameRules: [
                v => !!v || 'Ce champs est requis',
                v => v.length >= 1 || 'Ce champs doit au moins contenir un caractère'
            ],
            singleSelectRules: [
                v => !!v || 'Ce champs est requis',
            ],
            dateRules: [
                v => !!v || 'Ce champs est requis',
            ],
            numberRules: [
                v => !!v || 'Ce champs est requis',
                v => !!v && !isNaN(v) || 'Ce champs doit contenir un nombre'
            ],
        }),
        watch: {
            searchEntreprise(val) {
                val && this.queryEntreprises(val)
            },
            searchContact(val) {
                val && this.queryContacts(val)
            },
            searchFormation(val) {
                val && this.queryFormations(val)
            },
            searchFormateur(val) {
                val && this.queryFormateurs(val)
            },
            searchSalle(val) {
                val && this.querySalles(val)
            },

        },
        computed: {
            isNewEntry() {
                return this.editedIndex === -1;
            },
            formTitle() {
                return this.editedIndex === -1 ? 'Ajouter une session' : 'Modifier une session';
            },
            dialog: {
                get() {
                    return this.$store.state.session.dialog;
                },
                set(value) {
                    this.SET_DIALOG(value);
                }
            },
            deleteDialog: {
                get() {
                    return this.$store.state.session.deleteDialog;
                },
                set(value) {
                    this.SET_DELETE_DIALOG(value);
                }
            },
            id: {
                get() {
                    return this.$store.state.session.editedItem.id;
                }
            },
            entreprise: {
                get() {
                    return this.$store.state.session.editedItem.entreprise;
                },
                set(value) {
                    console.log(value);
                    this.MERGE_EDIT_TO_ITEM({ payload: { entreprise: value } });
                }
            },
            contact: {
                get() {
                    return this.$store.state.session.editedItem.contact;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { contact: value } });
                }
            },
            formation: {
                get() {
                    return this.$store.state.session.editedItem.formation;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { formation: value } });
                }
            },
            formateur: {
                get() {
                    return this.$store.state.session.editedItem.formateur;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { formateur: value } });
                }
            },
            salle: {
                get() {
                    return this.$store.state.session.editedItem.salle;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { salle: value } });
                }
            },
            dateClient: {
                get() {              
                    return this.$store.state.session.editedItem.dateClient;
                },
                set(value) {
                    console.log(value);
                    this.MERGE_EDIT_TO_ITEM({ payload: { dateClient: value } });
                    this.MERGE_EDIT_TO_ITEM({ payload: { date: moment.utc(String(value + " " + this.timeClient)).format() } });
                }
            },
            timeClient: {
                get() {
                    return this.$store.state.session.editedItem.timeClient;
                },
                set(value) {
                    console.log(value);

                    this.MERGE_EDIT_TO_ITEM({ payload: { timeClient: value } });
                    this.MERGE_EDIT_TO_ITEM({ payload: { date: moment.utc(String(this.dateClient + " " + value)).format() } });
                }
            },
            date: {
                get() {
                    return this.$store.state.session.editedItem.date;
                }
            },
            bonCommande: {
                get() { return this.$store.state.session.editedItem.bonCommande },
                set(value) { this.MERGE_EDIT_TO_ITEM({ payload: { bonCommande: value } }); }
            },
            typeFacturation: {
                get() { return this.$store.state.session.editedItem.typeFacturation },
                set(value) { this.MERGE_EDIT_TO_ITEM({ payload: { typeFacturation: value } }); }
            },
            utiliseDureeSession: {
                get() { return this.$store.state.session.editedItem.utiliseDureeSession },
                set(value) { this.MERGE_EDIT_TO_ITEM({ payload: { utiliseDureeSession: value } }); }
            },
            duree: {
                get() { return this.$store.state.session.editedItem.duree },
                set(value) { this.MERGE_EDIT_TO_ITEM({ payload: { duree: value } }); }
            },
            utilisePrixSession: {
                get() { return this.$store.state.session.editedItem.utilisePrixSession },
                set(value) { this.MERGE_EDIT_TO_ITEM({ payload: { utilisePrixSession: value } }); }
            },
            prix: {
                get() { return this.$store.state.session.editedItem.prix },
                set(value) { this.MERGE_EDIT_TO_ITEM({ payload: { prix: value } }); }
            },
            nombrePersonnesAttendues: {
                get() { return this.$store.state.session.editedItem.nombrePersonnesAttendues },
                set(value) { this.MERGE_EDIT_TO_ITEM({ payload: { nombrePersonnesAttendues: value } }); }
            },
            ...mapState([module.editedIndex, module.editedItem,
                module.searchedEntreprises,
                module.searchedContacts,
                module.searchedFormations,
                module.searchedFormateurs,
                module.searchedSalles]),

        },
        methods:
        {
            ...mapMutations([module.SET_DIALOG, module.SET_DELETE_DIALOG, module.SET_EDITED_ITEM_DEFAULT, module.MERGE_EDIT_TO_ITEM]),
            ...mapActions([module.SAVE_ITEM, module.DELETE_ITEM, module.CLOSE_DIALOG,
                module.SEARCH_ENTREPRISES,
                module.SEARCH_CONTACTS,
                module.SEARCH_FORMATIONS,
                module.SEARCH_FORMATEURS,
                module.SEARCH_SALLES,
            ]),

            del(event) {
                this.DELETE_ITEM();
            },

            save() {
                if (this.$refs.form.validate()) {
                    this.SAVE_ITEM();
                }
            },

            close() {
                this.CLOSE_DIALOG();
            },

            queryEntreprises(v) {
                this.SEARCH_ENTREPRISES({ searchTerm: v })
            },
            queryContacts(v) {
                this.SEARCH_CONTACTS({ searchTerm: v, entreprise: this.entreprise})
            },
            queryFormations(v) {
                this.SEARCH_FORMATIONS({ searchTerm: v })
            },
            queryFormateurs(v) {
                this.SEARCH_FORMATEURS({ searchTerm: v, formation: this.formation })
            },
            querySalles(v) {
                this.SEARCH_SALLES({ searchTerm: v })
            },
        }
    }      
</script>
