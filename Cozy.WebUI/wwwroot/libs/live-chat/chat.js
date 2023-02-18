function chatBox() {
    this.context = {
        inbox: [
            //   {
            //     FriendId: 0,
            //     FriendName: "Demo",
            //     LastMessage: "bye",
            //     unread: 3,
            //     date: "2022-11-12 12:44",
            //   },
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

    this.reloadInbox = () => {
        let messageList = document.querySelector("#messagelist");

        console.log(messageList);

        if (messageList == undefined) return;

        messageList.innerHTML = "";

        this.context.inbox.forEach((item) => {
            console.log(item);
            let div = document.createElement("div");
            div.className = "media userlist-box";
            div.setAttribute("data-id", item.FriendId);
            div.setAttribute("title", item.FriendName);

            let a = document.createElement('a');
            a.className = 'media-left';
            a.innerHTML = `<img class="media-object img-radius img-radius" src="/admin/images/avatar-3.jpg" alt="Generic placeholder image ">
      <div class="live-status bg-success"></div>`;

            div.appendChild(a);

            let divMediaBody = document.createElement('div');
            divMediaBody.className = 'media-body';
            divMediaBody.innerHTML = `<div class="f-13 chat-header d-flex flex-column">${item.FriendName}
                <span>${item.LastMessage}</span>
                </div>`;

            div.appendChild(divMediaBody);

            messageList.appendChild(div);
        });
    }
}
