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

// Request permission to send notifications
messaging.requestPermission()
    .then(function () {
        console.log('Notification permission granted.');
        // Get the token
        return messaging.getToken();
    })
    .then(function (token) {
        console.log('FCM Token:', token);
        // Send the token to the server
        fetch('/Account/SaveFcmToken', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({ token: token })
        });
    })
    .catch(function (err) {
        console.log('Unable to get permission to notify.', err);
    });

// Handle incoming messages while the web app is in the foreground
messaging.onMessage(function (payload) {
    console.log('Message received. ', payload);
    // Customize notification here
    const notificationTitle = payload.notification.title;
    const notificationOptions = {
        body: payload.notification.body,
        icon: payload.notification.icon || '/firebase-logo.png' // Change the icon URL if needed
    };

    if (Notification.permission === 'granted') {
        var notification = new Notification(notificationTitle, notificationOptions);
    }
});

// Register service worker
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('/firebase-messaging-sw.js')
        .then(function (registration) {
            console.log('Service Worker registered with scope:', registration.scope);
        }).catch(function (err) {
            console.log('Service Worker registration failed:', err);
        });
}
