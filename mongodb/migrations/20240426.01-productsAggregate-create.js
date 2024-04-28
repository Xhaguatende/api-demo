
module.exports = {
  async up(db, client) {
    const session = client.startSession();
    try {
      await session.withTransaction(async () => {
        let envName = process.env.ENVIRONMENT_NAME || "Local";

        console.log("---");
        console.log("STARTING - Create 'productsAggregate' View");
        console.log("environment: " + envName); 
        
		const pipeline = [
			  {
				$lookup: {
				  from: "categories",
				  localField: "categoryId",
				  foreignField: "id",
				  as: "categoryDetails"
				}
			  },
			  {
				$unwind: {
				  path: "$categoryDetails",
				  preserveNullAndEmptyArrays: true
				}
			  },
			  {
				$addFields: {
				  "category.id": "$categoryDetails.id",
				  "category.name": "$categoryDetails.name",
				  "category.description": "$categoryDetails.description"
				}
			  },
			  {
				$project: {
				  categoryDetails: 0
				}
			  }
			];

			const options = {
			  viewOn: "products",
			  pipeline: pipeline,
			  collation: { locale: "en" }
			};

		await db.createCollection('productsAggregate', options);
		
     
        console.log("FINISHED - Create 'productsAggregate' View");
		console.log("");
      });
	  
    }catch(error){
      console.log(error);
    } 
    finally {
      console.log("finally");      
	  await session.endSession();
    }

  },

  async down(db, client) {
    // TODO write the statements to rollback your migration (if possible)
  }
};
