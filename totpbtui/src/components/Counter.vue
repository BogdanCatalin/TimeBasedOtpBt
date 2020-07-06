<template>
    <div v-if="loaded">
        <h5 v-if="!expired">Code available for: </h5>
        <h5 v-else>Code expired</h5>
        {{displayMinutes}} : {{displaySeconds}}
    </div>
</template>

<script>
export default {
    props: ['year', 'month', 'date', 'hour', 'minute', 'second', 'millisecond'],
    data: ()=> ({
        displayMinutes: 0,
        displaySeconds: 0,
        loaded: false,
        expired: false
    }),
    computed: {
        _seconds: () => 1000,
        _minutes() {
            return this._seconds * 60;
        },
        _hours() {
            return this._minutes * 60;
        },
        _days() {
            return this._hours * 24;
        },
        end() {
            return new Date(
                this.year,
                this.month,
                this.date,
                this.hour,
                this.minute,
                this.second,
                this.millisecond
            );
        }
    },
    mounted() {
        this.showRemaining();
    },
    methods: {
        showRemaining() {
            const timer = setInterval( () => {
                const now = new Date();
                // const end = new Date(2020, 6, 6, 18, 30, 10, 10);
                const distance = this.end.getTime() - now.getTime();
                
                if(distance < 0) {
                    clearInterval(timer);
                    this.expired = true;
                    return;
                }

                const days = Math.floor(distance / this._days);
                const hours = Math.floor((distance & this._days) / this._hours);
                const minutes = Math.floor((distance % this._hours) / this._minutes);
                const seconds = Math.floor((distance % this._minutes) / this._seconds);

                this.displayMinutes = minutes < 10 ? "0" + minutes : minutes;
                this.displaySeconds = seconds < 10 ? "0" + seconds : seconds;

                this.loaded = true;

            }, 1000);
        }
    }
}
</script>

<style lang="scss" scoped>
</style>