<template>
    <div>
        <h2>Totp Password</h2>
        <form @submit.prevent="handleSubmit">
            <div class="form-group">
                <label for="totppassword"></label>
                <input type="text" v-model="totppassword" name="totppassword" class="form-control" :class="{ 'is-invalid': submitted && !totppassword }" />
                <div v-show="submitted && !totppassword" class="invalid-feedback">Totp Password is required</div>
            </div>
            <div class="form-group">
                <label for="totppasswordValue">Login One Time Password: {{totppasswordValue}}</label>
            </div>
            <Counter
            :year="year"
            :month="month"
            :date="date"
            :hour="hour"
            :minute="minute"
            :second="second"
            :millisecond="millisecond"
            />

            <div class="form-group">
                <button class="btn btn-primary" :disabled="status.loggingIn">Login</button>
                <img v-show="status.loggingIn" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                <router-link to="/register" class="btn btn-link">Register</router-link>
            </div>
        </form>
    </div>
</template>

<script>
import { mapState, mapActions } from 'vuex'
import Counter from '../components/Counter.vue'

export default {
    data () {
        return {
            totppassword: '',
            submitted: false,
            totppasswordValue: this.$store.state.account.user.totpPassword,
            remainingTime: this.$store.state.account.user.remainingTime,
            year: 0,
            month: 0,
            date: 0,
            hour: 0,
            minute: 0,
            second: 0,
            millisecond: 0
        }
    },
    computed: {
        ...mapState('account', ['status'])
    },
    created () {

    },
    mounted() {
        this.getTimeComponents();
    },
    methods: {
        ...mapActions('account', ['authenticate']),
        handleSubmit (e) {
            this.submitted = true;
            const { totppassword } = this;
            if (totppassword) {
                this.authenticate({ totppassword })
            }
        },
        getTimeComponents() {
            
            const now = new Date();
            const timeToExpire = new Date(now.getTime() + (this.remainingTime * 1000));
            
            this.year = timeToExpire.getFullYear();
            this.month = timeToExpire.getMonth();
            this.date = timeToExpire.getDate();
            this.hour = timeToExpire.getHours();
            this.minute = timeToExpire.getMinutes();
            this.second = timeToExpire.getSeconds();
            this.millisecond = timeToExpire.getMilliseconds();
        }
    },

    components: {
        Counter
    }
};
</script>