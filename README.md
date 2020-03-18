# DeathAndTaxes
Hi everyone. I'm Oak, and I did the coding for Death and Taxes, a game that released about a month ago. It was developed over the years by various people, but a final team of 10 people got the game out of the gate on 20th of February this year (2020, in case you have trouble remembering which year is it, like I do).

Early on, when I joined the team, I had two wishes for the game.

My first wish was to release a free torrent version of the game, which has now been done thanks to the awesome people at /r/crackwatch, 1337x and RARBG.

My second wish was to release the entire codebase of the game as open source. As you can see.

By now you're probably thinking: why? Probably "WHY!?" even, to both of those wishes.


So... Why?

Ever since I started coding, I have absolutely loved open source stuff. And I mean open source by the definition of the Open Source Initiative [https://opensource.org/]. I realize that releasing *only* the codebase is not exactly true in verbatim to the principles of FOSS stuff, but this is a deliberate decision. Making the code open source is fine by us, but making the art and other such assets is not. I won't go too much into detail to avoid opening a weird legal/ethical/whatever can of worms.

That being said - we're not afraid that people would copy the game or whatnot. The game has already been made, is shipped, has sold pretty well (26k copies at time of writing! Woop!) and hell.. if you're inspired by it... go for it! Make games! MAKE ALL THE GAMES! And make them well. We need more good games that respect our players' time, capability and intelligence.

Now that we've got that out of the way, I'll just casually start answering the question I postulated earlier. This is going to be long, but please bear with me, it's important (at least to me).

In addition to having the utmost respect for open-source/FOSS projects, I have this dream that I want to keep alive. The dream of making games. Right now, being in a most serendipitous position, I can develop games. I also want other people to make games. And I definitely want to learn how to make even better games myself. When I started out in this industry, back in 2013 with my first job, the situation was strange. Everybody was reinventing the wheel every time making a new game, be it the same studio using the same engine or various studios using various engines... and honestly, the situation hasn't changed that much. At least in principle. When I was learning how to "maek gaem", resources were few and far inbetween. Most of my experience came from being able to learn from my coworkers and having a diligent presence online, assimilating knowledge where I could. Bear in mind that I've mostly done coding, and this is what I focused on at the time.

Resources which I sorely missed were concrete, specific and (most importantly) helpful examples of real-world applications of coding principles, workarounds, hacks, engine-specific behaviour, et cetera. The most helpful places were, for example, the Unity Answer Hub, Stack Overflow, the Unreal Developers' Network... they all, at best, offered a window to see how people actually implement stuff. I am a firm believer that it is usually more efficient to learn from the mistakes of others, rather than just people having to make the same mistakes over and over again due to lack of reliable information.

And this leads me to the (incredibly winding and convoluted, sorry) answer to why we're releasing the source code for our game. I want to provide something that people can learn from. Including myself. The code for Death and Taxes is by NO MEANS clean or even "good" (depends on what your standard is). But the most important thing is that the code worked. The game shipped, and the game worked.

Well.. mostly. I still had to fix a cavalcade of game-breaking bugs, but after ~10 days of bugfixing, everything seems to be in order now. Granted, we did not have a large QA capacity. In fact, we didn't have any dedicated QA going over our game. We did, our community did, people who played the demo did. That was pretty much it. It's not much to work on, but it was enough.

I fully expect criticisms for the architecture, coding patterns, naming conventions, and so on, which is all very welcome. Having a constructive conversation with people so that you could learn or that I could improve is something that's actually important to me. So... if you have any questions or remarks, lets talk!

I do understand that there is limited usefulness for the source code without the assets and all of the Unity side set-up. But that's also not the point. The point here is to share the code for enthusiasts or other interested parties who want to inspect how the game was built and what state a released codebase of a game is. I removed some of the things that I couldn't keep in the repo due to potential licensing issues, but TwitchLib is under MIT license (like this repo) and all of the articy:draft stuff has been OK'd by the company and it has its own licenses, and all of that is included.

The code is far from perfect, the game is far from perfect, but I hope that it's an example that people can go through and have even just a little bit of an insight into what goes into making a game. Hopefully it helps.

Good luck, and have fun!
