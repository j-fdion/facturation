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
                                <v-text-field label="Prenom" :rules="nameRules" v-model="prenom"></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field label="Nom" :rules="nameRules" v-model="nom"></v-text-field>
                            </v-flex>
                            <v-flex xs12>
                                <v-select label="Rechercher une formation"
                                          autocomplete
                                          chips
                                          required
                                          :items="searchedFormations"
                                          clearable
                                          multiple
                                          item-text="formation.titre"
                                          item-value="formation.id"
                                          return-object
                                          :rules="selectRules"
                                          :search-input.sync="search"
                                          v-model="formateurFormations">
                                    <template slot="selection" slot-scope="data">
                                        <v-chip close
                                                @input="data.parent.selectItem(data.item)"
                                                :selected="data.selected"
                                                class="chip--select-multi"
                                                :key="JSON.stringify(data.item)">
                                            {{ data.item.formation.titre }}
                                        </v-chip>
                                    </template>
                                    <template slot="item" slot-scope="data">
                                        <template v-if="typeof data.item !== 'object'">
                                            <v-list-tile-content v-text="data.item"></v-list-tile-content>
                                        </template>
                                        <template v-else>
                                            <v-list-tile-content>
                                                <v-list-tile-title v-html="data.item.formation.titre"></v-list-tile-title>
                                            </v-list-tile-content>
                                        </template>
                                    </template>
                                </v-select>
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
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('formateur')

    import * as module from '../../store/modules/formateur/formateur'
    import moment from 'moment'

    export default {
        data: () => ({
            search: null,
            valid: true,
            nameRules: [
                v => !!v || 'Ce champs est requis',
                v => v.length >= 1 || 'Ce champs doit au moins contenir un caractère'
            ],
            selectRules: [
                v => !!v || 'Ce champs est requis',
                v => v.length >= 1 || 'Vous devez faire au moins une sélection'
            ],
        }),
        watch: {
            search(val) {
                val && this.querySelections(val)
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'Ajouter un formateur' : 'Modifier un formateur'
            },
            dialog: {
                get() {
                    return this.$store.state.formateur.dialog;
                },
                set(value) {
                    this.SET_DIALOG(value);
                }
            },
            deleteDialog: {
                get() {
                    return this.$store.state.formateur.deleteDialog;
                },
                set(value) {
                    this.SET_DELETE_DIALOG(value);
                }
            },
            prenom: {
                get() {
                    return this.$store.state.formateur.editedItem.prenom;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { prenom: value } });
                }
            },
            nom: {
                get() {
                    return this.$store.state.formateur.editedItem.nom;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { nom: value } });
                }
            },
            formateurFormations: {
                get() {
                    return this.$store.state.formateur.editedItem.formateurFormations;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { formateurFormations: value } });
                }
            },
            ...mapState([module.editedIndex, module.editedItem, module.searchedFormations]),

        },
        methods:
        {
            ...mapMutations([module.SET_DIALOG, module.SET_DELETE_DIALOG, module.SET_EDITED_ITEM_DEFAULT, module.MERGE_EDIT_TO_ITEM]),
            ...mapActions([module.SAVE_ITEM, module.DELETE_ITEM, module.CLOSE_DIALOG, module.SEARCH_FORMATIONS]),

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
                this.SEARCH_FORMATIONS({ searchTerm: v })
            }
        }
    }      
</script>
