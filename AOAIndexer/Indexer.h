// Indexer.h

#ifndef _INDEXER_h
#define _INDEXER_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

class IndexerClass
{
 protected:


 public:
	void init();
};

extern IndexerClass Indexer;

#endif

