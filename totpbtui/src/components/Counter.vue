<template>
    <div>
        <section class="text-3xl flex justify-center flex-col mx-auto text-center"></section>
        <section class="flex text-6xl justify-center content-center">
            <div class="minutes mx-2 relative">
                {{displayMinutes}}
                <div class="label text-sm absolute bottom-0">minutes</div>
            </div>
        </section>
        Hi from counterrr!
        {{displayMinutes}}
        {{displaySeconds}}
    </div>
</template>

<script>
export default {
    data: ()=> ({
        displayMinutes: 0,
        displaySeconds: 0
    })
}
</script>