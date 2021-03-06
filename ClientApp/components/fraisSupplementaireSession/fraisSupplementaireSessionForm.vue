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
                            <v-flex xs12>
                                <v-select label="Rechercher un frais supplémentaire"
                                          autocomplete
                                          required
                                          :items="fraisSupplementaires"
                                          clearable
                                          item-text="nom"
                                          item-value="id"
                                          return-object
                                          :rules="selectRules"
                                          :search-input.sync="searchFraisSupplementaire"
                                          v-model="fraisSupplementaire">
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
                                <v-text-field label="Quantité" required v-model="quantite" :rules="numberRules"></v-text-field>
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
                <span class="headline">Supprimer un frais supplémentaire</span>
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
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('fraisSupplementaireSession')

    import * as module from '../../store/modules/fraisSupplementaireSession/fraisSupplementaireSession'
    import moment from 'moment'

    export default {
        props: ['id'],
        data: () => ({
            searchFraisSupplementaire: null,
            valid: true,
            nameRules: [
                v => !!v || 'Ce champs est requis',
                v => v.length >= 1 || 'Ce champs doit au moins contenir un caractère'
            ],
            selectRules: [
                v => !!v || 'Ce champs est requis',
            ],
            dateRules: [
                v => !!v || 'Ce champs est requis'
            ],
            numberRules: [
                v => !!v || 'Ce champs est requis',
                v => !!v && !isNaN(v) || 'Ce champs doit contenir un nombre'
            ],
        }),
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'Ajouter un frais supplémentaire' : 'Modifier un frais supplémentaire'
            },
            dialog: {
                get() {
                    return this.$store.state.fraisSupplementaireSession.dialog;
                },
                set(value) {
                    this.SET_DIALOG(value);
                }
            },
            deleteDialog: {
                get() {
                    return this.$store.state.fraisSupplementaireSession.deleteDialog;
                },
                set(value) {
                    this.SET_DELETE_DIALOG(value);
                }
            },
            sessionId: {
                get() {
                    return this.id;
                }
            },
            fraisSupplementaire: {
                get() {
                    return this.$store.state.fraisSupplementaireSession.editedItem.fraisSupplementaire;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { fraisSupplementaireId: value.id } });
                    this.MERGE_EDIT_TO_ITEM({ payload: { fraisSupplementaire: value } });
                }
            },
            quantite: {
                get() {
                    return this.$store.state.fraisSupplementaireSession.editedItem.quantite;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { quantite: value } });
                }
            },
            ...mapState([module.editedIndex, module.editedItem, module.fraisSupplementaires])

        },
        watch:
        {
            searchFraisSupplementaire(val) {
                val && this.queryFraisSupplementaires(val)
            },
        },
        methods:
        {
            ...mapMutations([module.SET_DIALOG, module.SET_DELETE_DIALOG, module.SET_EDITED_ITEM_DEFAULT, module.MERGE_EDIT_TO_ITEM]),
            ...mapActions([module.SAVE_ITEM, module.DELETE_ITEM, module.CLOSE_DIALOG, module.SEARCH_FRAIS_SUPPLEMENTAIRES]),

            del(event) {
                this.DELETE_ITEM();
            },

            save() {
                if (this.$refs.form.validate()) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { sessionId: this.sessionId } });
                    this.SAVE_ITEM();
                }
            },

            close() {
                this.CLOSE_DIALOG();
            },

            queryFraisSupplementaires(v) {
                this.SEARCH_FRAIS_SUPPLEMENTAIRES({ searchTerm: v })
            }
        }
    }      
</script>
