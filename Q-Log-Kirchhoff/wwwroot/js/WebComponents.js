 

class TimeElapsed extends HTMLElement {
      
    constructor() {
        super();    
       }
    connectedCallback() {
        var registTime = new Date(this.getAttribute('date'));
        const now = new Date();

        const timeLapsed = this.diffMinutes(now, registTime);
  
        this.innerHTML = timeLapsed+" ----- min";
        //timer = setInterval(function () {
        //    timeBetweenDates(this.getAttribute('date'));
        //}, 1000);
    }


    diffMinutes(dt2, dt1) {

        var diff = (dt2.getTime() - dt1.getTime()) / 1000;
        diff /= 60;
        return Math.abs(Math.round(diff));

    }
}

if (!customElements.get('time-elapsed-tag')) {
    customElements.define('time-elapsed-tag', TimeElapsed);
}


//function timeBetweenDates(toDate) {
//    debugger;
//    var dateEntered = toDate;
//    var now = new Date();
//    var difference = dateEntered.getTime() - now.getTime();

//    if (difference <= 0) {

//        // Timer done
//        clearInterval(timer);

//    } else {

//        var seconds = Math.floor(difference / 1000);
//        var minutes = Math.floor(seconds / 60);
//        var hours = Math.floor(minutes / 60);
//        var days = Math.floor(hours / 24);

//        hours %= 24;
//        minutes %= 60;
//        seconds %= 60;

//        this.innerHTML = "<h1>" + minutes + "</h1>";
//    }
//}