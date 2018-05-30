<template>
    <div>
        <session-form>
        </session-form>
        <v-card>
            <full-calendar ref="calendar" :config="config" :events="events"
                           @event-selected="eventSelected"
                           @event-drop="eventDrop"
                           @event-resize="eventResize"
                           @event-created="select" />
        </v-card>
    </div>
</template>

<script>

    import { createNamespacedHelpers } from 'vuex'
    const { mapState, mapActions, mapGetters, mapMutations } = createNamespacedHelpers('session')

    import * as module from '../../store/modules/session/session'
    import axios from 'axios'


    export default {
        name: "hello",
        data() {
            return {
                events: function (start, end, timezone, callback) {
                    //console.log("START", start.utc().format(), "END", end.utc().format())
                    axios.get('/api/Gestion/LoadSessionEvents', {
                        params: {
                            start: start.utc().format(),
                            end: end.utc().format()
                        }
                    }).then(function (response) {
                        console.log(response);
                        callback(response.data);
                    }, (err) => {
                        console.log(err)
                    })
                },
                config: {
                    schedulerLicenseKey: "GPL-My-Project-Is-Open-Source",
                    defaultView: "timelineDay",
                    locale: "fr",
                    header: {
                        left: "prev,next",
                        center: "title",
                        right: "timelineDay,timelineWeek,timelineMonth"
                    },
                    slotDuration: "00:05:00",
                    minTime: "07:00:00",
                    maxTime: "21:00:00",
                    selectMinDistance: 10,
                    eventLimit: false,
                    resourceLabelText: "Formateurs",
                    resources: function (callback, start, end, timezone) {
                        axios.get('/api/Gestion/LoadSessionRessources')
                            .then(function (response) {
                                console.log(response);
                                callback(response.data);
                            }, (err) => {
                                console.log(err)
                            })
                    }
                }
            };
        },
        methods: {
            ...mapActions([module.LOAD_ITEMS, module.SAVE_ITEM, module.OPEN_DIALOG_WITH_DATE, module.OPEN_DIALOG, module.OPEN_DELETE_DIALOG]),
            eventSelected(event) {
                this.OPEN_DIALOG({ payload: event.session });
            },
            eventDrop(event) {
                event.session.date = event.start.utc().format()
                self = this
                axios.post('/api/Gestion/SaveSession', JSON.parse(JSON.stringify(event.session)))
                    .then(function (response) {
                        self.refreshEvents()
                    }, (err) => {
                        console.log(err)
                    })
            },
            eventResize(event) {
                var duration = moment.duration(event.end.diff(event.start));
                var hours = duration.asHours();
                event.session.date = event.start.utc().format()
                event.session.duree = hours;
                event.session.utiliseDureeSession = true
                self = this
                Vue.$authentication.acquireToken().then(token => {
                    axios.post('/api/Gestion/SaveSession', JSON.parse(JSON.stringify(event.session)),
                        {
                            'Content-Type': 'application/json',
                            'Ocp-Apim-Subscription-Key': 'api key',
                            'Authorization': 'Bearer ' + token
                        })
                        .then(function (response) {
                            self.refreshEvents()
                        }, (err) => {
                            console.log(err)
                        })
                });
            },
            select(start, end, jsEvent, view, resource) {
                this.OPEN_DIALOG_WITH_DATE({ payload: start.start.utc().format() });
            },
            refreshEvents() {
              this.$refs.calendar.$emit('refetch-events')
            }
        }
    };
</script>

<style>
.fc-day-grid-event > .fc-content {
    white-space: normal;
}
</style>
