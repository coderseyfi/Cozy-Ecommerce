function chatBox() {
    this.context = {
        inbox: [],
    };

    this.receiveMessage = (inboxMessage) => {
        let anotherMessages = this.context.inbox.filter(
            (i) => i.FriendId != inboxMessage.FriendId
        );

        anotherMessages.unshift(inboxMessage);
        this.context.inbox = anotherMessages;

        console.log(this.context.inbox);

        this.reloadInbox();
    }

    this.reloadInbox = () => {
        let messageList = document.querySelector("#messagelist");

        console.log(messageList);

        if (messageList == undefined) return;

        messageList.innerHTML = "";

        this.context.inbox.forEach((item) => {
            console.log(item);

            var div = document.createElement('div');

            div.setAttribute('class', 'media userlist-box');
            div.setAttribute('data-id', item.FriendId);
            div.setAttribute('data-status', 'online');
            div.setAttribute('data-username', item.FriendName);
            div.setAttribute('data-toggle', 'tooltip');
            div.setAttribute('data-placement', 'left');

            div.setAttribute('title', item.FriendName);


            let divMediaBody = document.createElement('div');
            divMediaBody.className = 'f-13 chat-header';
            divMediaBody.innerHTML = `
                     <div class="f-13 chat-header">
                          ${item.FriendName}
                     </div>
                     <div class="user-message"> 
                          ${item.LastMessage}
                     </div>
                    `;

            div.appendChild(divMediaBody);

            messageList.appendChild(div);
        });
    }
} // <-- Added closing brace here.






function chatBoxAdmin() {
    this.context = {
        inbox: [
               {
                 FriendId: 0,
                 FriendName: "Demo",
                 LastMessage: "bye",
                 unread: 3,
                 date: "2022-11-12 12:44",
               },
        ],
    };

    this.receiveMessage = (inboxMessage) => {
        let anotherMessages = this.context.inbox.filter(
            (i) => i.FriendId != inboxMessage.FriendId
        );

        anotherMessages.unshift(inboxMessage);
        this.context.inbox = anotherMessages;

        console.log(this.context.inbox);

        this.reloadInbox();
    }
}