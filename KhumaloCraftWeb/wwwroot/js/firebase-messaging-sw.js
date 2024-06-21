importScripts('https://www.gstatic.com/firebasejs/8.6.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.6.1/firebase-messaging.js');

// Your web app's Firebase configuration
var firebaseConfig = {
    apiKey: "AIzaSyDjX4h9Iapv4Y3hSmS72XkzTpXnYBxpDzE",
    authDomain: "khumalocraft-e650c.firebaseapp.com",
    projectId: "khumalocraft-e650c",
    storageBucket: "khumalocraft-e650c.appspot.com",
    messagingSenderId: "329126009785",
    appId: "1:329126009785:web:30ae9727474be027ceb468"
};

// Initialize Firebase
firebase.initializeApp(firebaseConfig);

const messaging = firebase.messaging();

// Handle background messages
messaging.onBackgroundMessage(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    const notificationTitle = payload.notification.title;
    const notificationOptions = {
        body: payload.notification.body,
        icon: payload.notification.icon || '/firebase-logo.png' // Change the icon URL if needed
    };

    self.registration.showNotification(notificationTitle, notificationOptions);
});
