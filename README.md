# Cloud-Ark-Sim

This is a simulator of the "Cloud Ark" from the science fiction novel, "Seven Eves". In the book, a fleet of thousands of ships are launched into 
orbit to form a "Cloud Ark", a "formation" of these ships, similar to a Starlink Train. This sim is fairly high fidelity, simulating the ISP of the 
thrusters, the fuel & oxidizer they use, how the ships mass changes over time as they expend fuel, and how that then goes on to affect their moment 
of inertia when manuvering. Manuvering of the ships uses a PID controller to allow for smooth orientation changes.


## Ship Structure
Each ship is assumed to be cylindrical, with 2 "Thruster Rings", one at the front and one at the back. A thruster ring is a ring of 4 "Thruster Quads", located on the outside of the ship 90 degrees apart from each other. Each thruster quad has 4 thrusters oriented 90 degrees apart (fore, port, aft, starbord), with all 4 sharing the same fuel & oxidizer tanks.

Ships also have a ShipFlightComputer, which is actually controlls the ships functions.
