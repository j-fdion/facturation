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
                            <v-flex xs6>
                                <v-text-field label="# Téléphone 1" :rules="nameRules" required v-model="noTelephone1"></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field label="# Téléphone 2" v-model="noTelephone2"></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field label="Email" v-model="email"></v-text-field>
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
                <span class="headline">Supprimer un contact</span>
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
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('contact')

    import * as module from '../../store/modules/contact/contact'
    import moment from 'moment'

    export default {
        data: () => ({
            search: null,
            valid: true,
            nameRules: [
                v => !!v || 'Ce champs est requis',
                v => v.length >= 1 || 'Ce champs doit au moins contenir un caractère'
            ]
        }),
        watch: {
            search(val) {
                val && this.querySelections(val)
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? "Ajouter un contact" : "Modifier un contact"
            },
            dialog: {
                get() {
                    return this.$store.state.contact.dialog;
                },
                set(value) {
                    this.SET_DIALOG(value);
                }
            },
            deleteDialog: {
                get() {
                    return this.$store.state.contact.deleteDialog;
                },
                set(value) {
                    this.SET_DELETE_DIALOG(value);
                }
            },
            prenom: {
                get() {
                    return this.$store.state.contact.editedItem.prenom;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { prenom: value } });
                }
            },
            nom: {
                get() {
                    return this.$store.state.contact.editedItem.nom;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { nom: value } });
                }
            },
            noTelephone1: {
                get() {
                    return this.$store.state.contact.editedItem.noTelephone1;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { noTelephone1: value } });
                }
            },
            noTelephone2: {
                get() {
                    return this.$store.state.contact.editedItem.noTelephone2;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { noTelephone2: value } });
                }
            },
            email: {
                get() {
                    return this.$store.state.contact.editedItem.email;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { email: value } });
                }
            },
            commentaire: {
                get() {
                    return this.$store.state.contact.editedItem.commentaire;
                },
                set(value) {
                    this.MERGE_EDIT_TO_ITEM({ payload: { commentaire: value } });
                }
            },
            ...mapState([module.editedIndex, module.editedItem]),

        },
        methods:
        {
            ...mapMutations([module.SET_DIALOG, module.SET_DELETE_DIALOG, module.SET_EDITED_ITEM_DEFAULT, module.SET_ENTREPRISE, module.MERGE_EDIT_TO_ITEM]),
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
