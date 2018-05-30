<template>
    <v-dialog v-if="dialog" v-model="dialog" max-width="700px">
        <v-card>
            <v-card-title>
                <span class="headline">{{ formTitle }}</span>
            </v-card-title>
            <v-card-text style="min-height: 375px;">
                <v-container grid-list-md>
                    <v-form v-model="valid" ref="form" lazy-validation>
                        <v-layout wrap>
                            <v-flex xs6>
                                <v-text-field label="Prénom" :rules="nameRules" required v-model="prenom"></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field label="Nom" :rules="nameRules" required v-model="nom"></v-text-field>
                            </v-flex>
                            <v-flex xs12>
                                <v-date-picker v-model="dateNaissance"
                                               required
                                               color="grey darken-4"
                                               :reactive="true"
                                               locale="fr-ca"
                                               full-width
                                               landscape>
                                </v-date-picker>
                            </v-flex>
                            <v-flex xs12>
                                <v-select label="Rechercher une entreprise"
                                          autocomplete
                                          required
                                          :items="entreprises"
                                          clearable
                                          item-text="nom"
                                          item-value="id"
                                          return-object
                                          :rules="selectRules"
                                          :search-input.sync="search"
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
                            <v-flex xs12>
                                <v-text-field label="Commentaire"
                                              multi-line
                                              v-model="commentaire">
                                </v-text-field>
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
                <span class="headline">Supprimer une personne</span>
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
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('personne')

    import * as module from '../../store/modules/personne/personne'
    import moment from 'moment'

    export default {
        data: () => ({
            search: null,
            valid: true,
            config: {
                useCurrent: false,
                format: "L",
                locale: "fr-ca",
                viewMode: 'decades'
            },
            nameRules: [
                v => !!v || 'Ce champs est requis',
                v => v.length >= 1 || 'Ce champs doit au moins contenir un caractère'
            ],
            selectRules: [
                v => !!v || 'Ce champs est requis',
            ],
            dateRules: [
                v => !!v || 'Ce champs est requis'
            ]
        }),
        watch: {
            search(val) {
                val && this.querySelections(val)
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'Ajouter une personne' : 'Modifier une personne'
            },
            dialog: {
                get() {
                    return this.$store.state.personne.dialog;
                },
                set(value) {
                    this.SET_DIALOG(value);
                }
            },
            deleteDialog: {
                get() {
                    return this.$store.state.personne.deleteDialog;
                },
                set(value) {
                    this.SET_DELETE_DIALOG(value);
                }
            },
            nom: {
                get() {
                    return this.$store.state.personne.editedItem.nom;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { nom: value } });
                }
            },
            prenom: {
                get() {
                    return this.$store.state.personne.editedItem.prenom;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { prenom: value } });
                }
            },
            commentaire: {
                get() {
                    return this.$store.state.personne.editedItem.commentaire;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { commentaire: value } });
                }
            },
            entreprise: {
                get() {
                    return this.$store.state.personne.editedItem.entreprise;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { entreprise: value } });
                }
            },
            dateNaissance: {
                get() {
                    if (this.$store.state.personne.editedItem.dateNaissance !== null) {
                        return moment.utc(this.$store.state.personne.editedItem.dateNaissance).locale("fr-ca").format('L');
                    }
                    else {
                        return moment().utc().locale("fr-ca").format('L')
                    }

                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { dateNaissance: value }});
                }
            },
            ...mapState([module.editedIndex, module.editedItem, module.entreprises]),

        },
        methods:
        {
            ...mapMutations([module.SET_DIALOG, module.SET_DELETE_DIALOG, module.SET_EDITED_ITEM_DEFAULT, module.SET_ENTREPRISE, module.MERGE_EDIT_TO_ITEM]),
            ...mapActions([module.SAVE_ITEM, module.DELETE_ITEM, module.CLOSE_DIALOG, module.SEARCH_ENTREPRISES]),

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

            querySelections(v) {
                this.SEARCH_ENTREPRISES({ searchTerm: v })
            }
        }
    }      
</script>
