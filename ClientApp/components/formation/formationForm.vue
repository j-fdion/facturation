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
                                <v-text-field label="Titre" :rules="nameRules" required v-model="titre"></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field label="Duree" suffix="heure(s)" :rules="priceRules" required v-model="duree"></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field label="Taux Horaire" suffix="$/heure" :rules="priceRules" required v-model="tauxHoraire"></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field label="Prix forfaitaire" suffix="$" :rules="priceRules" required v-model="prixForfaitaire"></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field label="Prix unitaire" suffix="$" :rules="priceRules" required v-model="prixUnitaire"></v-text-field>
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
                <span class="headline">Supprimer un type d'accessoire</span>
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
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('formation')

    import * as module from '../../store/modules/formation/formation'

    export default {
        data: () => ({
            search: null,
            valid: true,
            nameRules: [
                v => !!v || 'Ce champs est requis',
                v => v.length >= 1 || 'Ce champs doit au moins contenir un caractère'
            ],
            priceRules: [
                v => !isNaN(v) || 'Ce champs doit contenir un nombre'
            ],
        }),
        watch: {
            search(val) {
                val && this.querySelections(val)
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? "Ajouter une formation" : "Modifier une formation"
            },
            dialog: {
                get() {
                    return this.$store.state.formation.dialog;
                },
                set(value) {
                    this.SET_DIALOG(value);
                }
            },
            deleteDialog: {
                get() {
                    return this.$store.state.formation.deleteDialog;
                },
                set(value) {
                    this.SET_DELETE_DIALOG(value);
                }
            },
            titre: {
                get() {
                    return this.$store.state.formation.editedItem.titre;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { titre: value } });
                }
            },
            duree: {
                get() {
                    return this.$store.state.formation.editedItem.duree;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { duree: value } });
                }
            },
            tauxHoraire: {
                get() {
                    return this.$store.state.formation.editedItem.tauxHoraire;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { tauxHoraire: value } });
                }
            },
            prixForfaitaire: {
                get() {
                    return this.$store.state.formation.editedItem.prixForfaitaire;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { prixForfaitaire: value } });
                }
            },
            prixUnitaire: {
                get() {
                    return this.$store.state.formation.editedItem.prixUnitaire;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { prixUnitaire: value } });
                }
            },
            ...mapState([module.editedIndex, module.editedItem]),

        },
        methods:
        {
            ...mapMutations([module.SET_DIALOG, module.SET_DELETE_DIALOG, module.SET_EDITED_ITEM_DEFAULT, module.MERGE_EDIT_TO_ITEM]),
            ...mapActions([module.SAVE_ITEM, module.DELETE_ITEM, module.CLOSE_DIALOG]),

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
            }
        }
    }      
</script>
